using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeetupAPI.Models
{
    public class BudgetDto
    {
        public string budgetId { get; set; }
        public string name { get; set; }
        public double totalBudgetAmount { get; set; }
        public double totalCostAmount { get; set; }
        public List<CostCategoryDto> costCategories { get; set; }
    }
}
