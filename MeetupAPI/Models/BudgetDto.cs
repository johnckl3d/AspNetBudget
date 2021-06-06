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
        public string description { get; set; }
        public double totalBudgetAmount { get; set; }
        public double totalCostAmount { get; set; }

        public List<CostSnapShotDto> costSnapShots { get; set; }

        public List<CostCategoryDto> costCategories { get; set; }


        public BudgetDto()
        {
            this.costSnapShots = new List<CostSnapShotDto>();
        }
        public void AddCostSnapShot(CostSnapShotDto snapShot)
        {
            costSnapShots.Add(snapShot);
        }
    }
}
