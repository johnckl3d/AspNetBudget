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
        public ActionResult<List<CostCategoryDto>> Get()
        {
           
            var budgets = _budgetContext.CostCategories.Include(b => b.costItems).ToList();
            var budgetDtos = _mapper.Map<List<CostCategoryDto>>(budgets);
            return Ok(budgetDtos);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post([FromBody] CostCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.budgetId == null)
            {
                return NotFound();
            }
            var budget = _budgetContext.Budgets
                .FirstOrDefault(c => c.budgetId.Replace(" ", "-") == model.budgetId.ToLower());
            if (budget == null)
            {
                return NotFound();
            }
            var costCategory = _mapper.Map<CostCategory>(model);
            var id = Guid.NewGuid().ToString();
            costCategory.costCategoryId = id;

            double totalAmount = 0;
            foreach (CostItem i in costCategory.costItems)
            {
                i.costItemId = Guid.NewGuid().ToString();
                totalAmount += i.amount;
            }

            costCategory.totalAmount = totalAmount;
            _budgetContext.CostCategories.Add(costCategory);

            _budgetContext.SaveChanges();

            return Created($"api/costCategory/{id}", null);
        }

        [HttpDelete("{costCategoryId}")]
        [AllowAnonymous]
        public ActionResult Delete(string costCategoryId)
        {

            var costCategory = _budgetContext.CostCategories.FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if (costCategory == null)
            {
                return NotFound();
            }
            _budgetContext.Remove(costCategory);


            _budgetContext.SaveChanges();

            return NoContent();
        }
    }
}
