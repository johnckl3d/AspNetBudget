using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class LoginResponseDto
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }


        public LoginResponseDto(string _accessToken, string _refreshToken)
        {
            accessToken = _accessToken;
            refreshToken = _refreshToken;
        }

    }
}