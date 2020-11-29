using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MeetupAPI.Entities;

namespace MeetupAPI.Models
{
    public class BudgetResponseDto
    {
        public string budgetId { get; set; }
        public string name { get; set; }
        public double totalBudgetAmount { get; set; }
        public double totalCostAmount { get; set; }

        public List<CostSnapShotDto> costSnapShots { get; set; }

        public List<CostCategory> costCategories { get; set; }


        public BudgetResponseDto(string _budgetId, string _name, double _totalBudgetAmount, double _totalCostAmount, List<CostSnapShotDto> _costSnapShots, List<CostCategory> _costCategories)
        {
            budgetId = _budgetId;
            name = _name;
            totalBudgetAmount = _totalBudgetAmount;
            totalCostAmount = _totalCostAmount;
            costSnapShots = _costSnapShots;
            costCategories = _costCategories;
            this.costSnapShots = new List<CostSnapShotDto>();
        }
        public void AddCostSnapShot(CostSnapShotDto snapShot)
        {
            costSnapShots.Add(snapShot);
        }
    }
}