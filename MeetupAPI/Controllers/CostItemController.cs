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
    [Route("api/costCategory/{costCategoryId}/costItem")]
    [Authorize]
    public class CostItemController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger _logger;


        public CostItemController(BudgetContext budgetContext, IMapper mapper, IAuthorizationService authorizationService, ILogger<CostItemController> logger)
        {
            _budgetContext = budgetContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<CostItemDto>> Get(string costCategoryId)
        {

            var costCategory = _budgetContext.CostCategories.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if (costCategory == null)
            {
                return NotFound();
            }
            var costItems = costCategory.costItems.ToList();
            var costItemDtos = _mapper.Map<List<CostItemDto>>(costItems);
            return Ok(costItemDtos);
        }

        [HttpPost]
        public ActionResult Post(string costCategoryId, [FromBody] CostItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var costCategory = _budgetContext.CostCategories.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
                if (costCategory == null)
                {
                    return NotFound();
                }

                var costItem = _mapper.Map<CostItem>(model);
                costItem.costItemId = Guid.NewGuid().ToString();
                costCategory.costItems.Add(costItem);

                double totalAmount = 0;
                foreach (CostItem i in costCategory.costItems)
                {
                    totalAmount += i.amount;
                }
                costCategory.totalAmount = totalAmount;

                _budgetContext.SaveChanges();

                return Created($"api/costItem/{costCategoryId}", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:CostItem:Create:{ex}");
                return BadRequest(ex);
            }
        }

        [HttpDelete("{costItemId}")]
        public ActionResult Delete(string costCategoryId, string costItemId)
        {
            var costCategory = _budgetContext.CostCategories.Include(b => b.costItems).FirstOrDefault(c => c.costCategoryId.Replace(" ", "-") == costCategoryId.ToLower());
            if (costCategory == null)
            {
                _logger.LogDebug($"Debug:Budget:Delete:costCategoryId:{costCategoryId}");
                return NotFound();
            }
            var costItem = costCategory.costItems.FirstOrDefault(c => c.costItemId == costItemId);

            if(costItem == null)
            {
                _logger.LogDebug($"Debug:Budget:Delete:costItemId:{costItemId}");
                return NotFound();
            }

            try
            {
                _budgetContext.CostItems.Remove(costItem);


                _budgetContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:CostItem:Delete:{ex}");
                return BadRequest(ex);
            }
        }


    }
}
