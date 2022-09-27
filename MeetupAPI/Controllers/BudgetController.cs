using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MeetupAPI.Authorization;
using MeetupAPI.Entities;
using MeetupAPI.Filters;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeetupAPI.Controllers
{
    [Route("api/budget")]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger _logger;

        public BudgetController(BudgetContext budgetContext, IMapper mapper, IAuthorizationService authorizationService, ILogger<BudgetController> logger)
        {
            _budgetContext = budgetContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpGet("retrieveList")]
        [Produces("application/json")]
        public ActionResult<List<BudgetDto>> Get()
        {
            var userid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var budget = _budgetContext.Budgets
                .Where(u => u.userId == userid)
                .Include(c => c.costCategories)
               .ThenInclude(i => i.costItems).ToList();

            //var budgetDto = _mapper.Map<Budget>(budget);
            List<BudgetDto> budgetDtoList =
    _mapper.Map<List<Budget>, List<BudgetDto>>(budget);
            List<BudgetResponseDto> result = new List<BudgetResponseDto>();
            foreach (BudgetDto item in budgetDtoList)
            {

                BudgetResponseDto responseDto = new BudgetResponseDto(item.budgetId, item.name, item.description, item.totalBudgetAmount, item.totalCostAmount, new List<CostSnapShotDto>(), new List<CostCategoryDto>());
                foreach (var c in item.costCategories)
                {
                    responseDto.AddCostCategory(c);
                    if (c.costItems != null)
                    {
                        foreach (var i in c.costItems)
                        {
                            CostSnapShotDto s = new CostSnapShotDto(i.dateTime, i.amount);
                            responseDto.AddCostSnapShot(s);
                        }
                        responseDto.ReorderSnapshotsByDateTime();
                    }
                }
                result.Add(responseDto);
            }
            //return Ok();
            return Ok(result);
        }

        [HttpGet("{budgetId}")]
        [AllowAnonymous]
        public ActionResult<BudgetDto> Get(string budgetId)
        {
            if (budgetId == null)
            {
                return NotFound();
            }
            var budget = _budgetContext.Budgets
                .Include(c => c.costCategories)
               .ThenInclude(i => i.costItems).ToList()
                .FirstOrDefault(c => c.budgetId.Replace(" ", "-") == budgetId.ToLower());

            var budgetDto = _mapper.Map<BudgetDto>(budget);
            BudgetResponseDto result = new BudgetResponseDto(budgetDto.name, budgetDto.name, budgetDto.description, budgetDto.totalBudgetAmount, budgetDto.totalCostAmount, new List<CostSnapShotDto>(), new List<CostCategoryDto>());
            foreach (var c in budgetDto.costCategories)
            {
                result.AddCostCategory(c);
                if (c.costItems != null)
                {
                    foreach (CostItemDto i in c.costItems)
                    {
                        CostSnapShotDto s = new CostSnapShotDto(i.dateTime, i.amount);
                        result.AddCostSnapShot(s);
                    }
                    result.ReorderSnapshotsByDateTime();
                }
            }
            return Ok(result);
            //return Ok();
        }


        [HttpPost]
        public ActionResult Post([FromBody] CreateBudgetRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogDebug($"Debug:Budget:Post:{model}");
            try
            {
                var budget = _mapper.Map<Budget>(model);
                var id = Guid.NewGuid().ToString();
                var userid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var user = _budgetContext.Users.Find(id);
                budget.createdBy = userid;
                budget.userId = userid;
                budget.User = user;
                budget.budgetId = id;
                budget.totalBudgetAmount = 0;
                budget.totalCostAmount = 0;

                _budgetContext.Add(budget);

                _budgetContext.SaveChanges();
                return Ok(id);
            }
            catch (Exception ex)
            {

                //_logger.LogWarning($"Warning:{userLoginDto.Userid}");
                _logger.LogError($"Error::Budget:Post:{ex}");
                //_logger.LogDebug($"userLoginDto{userLoginDto.Userid}");
                return BadRequest(ex);
            }
        }

        [HttpPut("{budgetId}")]
        public ActionResult Put(string budgetId, [FromBody] BudgetDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var budget = _budgetContext.Budgets.FirstOrDefault(b => b.budgetId.Replace(" ", "-").ToLower() == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(User, budget, new ResourceOperationRequirement(OperationType.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            _budgetContext.SaveChanges();

            return Created($"api/budget/{budgetId}", null);
        }


        [HttpDelete("{budgetId}")]
        public ActionResult Delete(string budgetId)
        {
            Console.WriteLine("Delete");
            var budget = _budgetContext.Budgets.FirstOrDefault(c => c.budgetId.Replace(" ", "-") == budgetId);
            if (budget == null)
            {
                Console.WriteLine("not found");
                return NotFound();
            }
            _budgetContext.Remove(budget);


            _budgetContext.SaveChanges();
            return Ok();
        }

    }
}
