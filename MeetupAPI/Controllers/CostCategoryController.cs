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
    [Route("api/costCategory")]
    [Authorize]
    public class CostCategoryController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public CostCategoryController(BudgetContext budgetContext, IMapper mapper, IAuthorizationService authorizationService)
        {
            _budgetContext = budgetContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<CostCategoryDetailsDto>> Get()
        {
           
            var budgets = _budgetContext.Budgets.Include(b => b.costItems).ToList();
            var budgetDtos = _mapper.Map<List<CostCategoryDetailsDto>>(budgets);
            return Ok(budgetDtos);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("{costCategoryId}/costitem")]
        public ActionResult Post(string costCategoryId, [FromBody] CostItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var budget = _budgetContext.Budgets.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if(budget == null)
            {
                return NotFound();
            }

            var costItem = _mapper.Map<CostItem>(model);
            budget.costItems.Add(costItem);
            _budgetContext.SaveChanges();

            return Created($"api/budget/{costCategoryId}", null);
        }


    }
}
