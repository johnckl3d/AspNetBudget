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
        public BudgetController(BudgetContext budgetContext, IMapper mapper, IAuthorizationService authorizationService)
        {
            _budgetContext = budgetContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<BudgetDto>> Get()
        {
            var budget = _budgetContext.Budgets
                .Include(c => c.costCategories)
               .ThenInclude(i => i.costItems).ToList();

            //var budgetDto = _mapper.Map<Budget>(budget);
            List<BudgetDto> budgetDtoList =
    _mapper.Map<List<Budget>, List<BudgetDto>>(budget);
            List<BudgetResponseDto> result = new List<BudgetResponseDto>();
            foreach (BudgetDto item in budgetDtoList)
            {

                BudgetResponseDto responseDto = new BudgetResponseDto(item.budgetId, item.name, item.totalBudgetAmount, item.totalCostAmount, new List<CostSnapShotDto>(), new List<CostCategoryDto>());
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
            BudgetResponseDto result = new BudgetResponseDto(budgetDto.name, budgetDto.name, budgetDto.totalBudgetAmount, budgetDto.totalCostAmount, new List<CostSnapShotDto>(), new List<CostCategoryDto>());
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
        [AllowAnonymous]
        public ActionResult Post([FromBody] BudgetDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var budget = _mapper.Map<Budget>(model);
            var id = Guid.NewGuid().ToString();
            budget.budgetId = id;

            
            _budgetContext.Add(budget);

            _budgetContext.SaveChanges();

            return Created($"api/budget/{id}", null);
        }


        [HttpDelete("{budgetId}")]
        [AllowAnonymous]
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

            return NoContent();
        }

        //[HttpDelete("{costCategoryId}")]
        //[AllowAnonymous]
        //public ActionResult Delete(string costCategoryId)
        //{

        //    var costCategory = _budgetContext.CostCategories.FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
        //    if (costCategory == null)
        //    {
        //        return NotFound();
        //    }
        //    _budgetContext.Remove(costCategory);


        //    _budgetContext.SaveChanges();

        //    return NoContent();
        //}
    }
}
