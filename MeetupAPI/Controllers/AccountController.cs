using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MeetupAPI.Entities;
using MeetupAPI.Helpers;
using MeetupAPI.Identity;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Entities;

namespace MeetupAPI.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly BudgetContext _meetupContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public AccountController(BudgetContext meetupContext, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider, IOptions<AppSettings> appSettings, ILogger<AccountController> logger)
        {
            _meetupContext = meetupContext;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]UserLoginDto userLoginDto)
        {
            _logger.LogDebug($"Debug:Login:{userLoginDto}");
            try {
                var user = _meetupContext.Users
                    .Include(user => user.Role)
                    .FirstOrDefault(user => user.userId == userLoginDto.Userid);
                if (user == null)
                {
                    return BadRequest("Invalid username or password");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.passwordHash, userLoginDto.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    return BadRequest("Invalid username or password");
                }

                var token = _jwtProvider.GenerateJwtToken(user);
                var refreshToken = _jwtProvider.GenerateJwtRefreshToken(userLoginDto.IPAddress);
                user.RefreshTokens.Add(refreshToken);
                RemoveOldRefreshTokens(user);
                _meetupContext.Update(user);
                _meetupContext.SaveChanges();

                var response = new LoginResponseDto(token, refreshToken.Token, user.userId, user.email);
                return Ok(response);
            } catch (Exception ex)
            {

                //_logger.LogWarning($"Warning:{userLoginDto.Userid}");
                _logger.LogError($"Error:{ex}");
                //_logger.LogDebug($"userLoginDto{userLoginDto.Userid}");
                return BadRequest(ex);
            }
        }


        private void RemoveOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        [HttpPost("refreshToken")]
        public ActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = _meetupContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == request.refreshToken));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.refreshToken);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = _jwtProvider.GenerateJwtRefreshToken(request.IPAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = request.IPAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _meetupContext.Update(user);
            _meetupContext.SaveChanges();

            // generate new jwt
            var jwtToken = _jwtProvider.GenerateJwtToken(user);

            var response = new RefreshTokenResponseDto(jwtToken, newRefreshToken.Token);
            return Ok(response);
        }

        [HttpPost("revokeToken")]
        public ActionResult RevokeToken([FromBody] UserLogoutRequestDto request)
        {
            try
            {
                var user = getUserByRefreshToken(request.RefreshToken);
                var refreshToken = user.RefreshTokens.Single(x => x.Token == request.RefreshToken);

                if (!refreshToken.IsActive)
                    throw new AppException("Invalid token");

                // revoke token and save
                revokeRefreshToken(refreshToken, request.IPAddress, "Revoked without replacement");
                _meetupContext.Update(user);
                _meetupContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                return BadRequest(ex);
            }
        }

        private User getUserByRefreshToken(string token)
        {
            var user = _meetupContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                throw new AppException("Invalid token");

            return user;
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtProvider.GenerateJwtRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }


        private IEnumerable<User> GetAll()
        {
            return _meetupContext.Users;
        }


        private User GetById(int id)
        {
            var user = _meetupContext.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody]RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new User()
            {
                userId = registerUserDto.userId,
                firstName = registerUserDto.firstName,
                lastName = registerUserDto.lastName,
                email = registerUserDto.email,
                roleId = registerUserDto.roleId
            };

            var passwordHash = _passwordHasher.HashPassword(newUser, registerUserDto.password);
            newUser.passwordHash = passwordHash;

            _meetupContext.Users.Add(newUser);
            _meetupContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize]
        public ActionResult DeleteAccount()
        {
            try
            {
                var userid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var user = _meetupContext.Users
            .Where(u => u.userId == userid);
                // return null if no user found with token

                if (user == null)
                {
                    return NotFound();
                }
                _meetupContext.RemoveRange(user);


                _meetupContext.SaveChanges();
                return Ok();
            } catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                return BadRequest(ex);
            }
        }
    }
}
