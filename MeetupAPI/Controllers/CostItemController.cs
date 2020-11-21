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
    [Route("api/costItem/{costCategoryId}/costItem")]
    [Authorize]
    public class CostItemController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public CostItemController(BudgetContext budgetContext, IMapper mapper, IAuthorizationService authorizationService)
        {
            _budgetContext = budgetContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post(string costCategoryId, [FromBody] CostItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var budget = _budgetContext.Budgets.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if (budget == null)
            {
                return NotFound();
            }

            var costItem = _mapper.Map<CostItem>(model);
            costItem.costItemId = Guid.NewGuid().ToString();
            budget.costItems.Add(costItem);
            _budgetContext.SaveChanges();

            return Created($"api/costItem/{costCategoryId}", null);
        }

        [HttpDelete]
        [AllowAnonymous]
        public ActionResult Delete(string costCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var budget = _budgetContext.Budgets.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if (budget == null)
            {
                return NotFound();
            }
            _budgetContext.CostItems.RemoveRange(budget.costItems);
            _budgetContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{costItemId}")]
        [AllowAnonymous]
        public ActionResult Delete(string costCategoryId, string costItemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var budget = _budgetContext.Budgets.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if (budget == null)
            {
                return NotFound();
            }
            var costItem = budget.costItems.FirstOrDefault(c => c.costItemId == costItemId);

            if(costItem == null)
            {
                return NotFound();
            }
            _budgetContext.CostItems.Remove(costItem);
            _budgetContext.SaveChanges();

            return NoContent();
        }
    }
}
