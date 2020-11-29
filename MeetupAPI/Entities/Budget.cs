﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeetupAPI.Entities
{
    public class Budget
    {
        [Key]
        public string budgetId { get; set; }
        public string name { get; set; }
        public double totalBudgetAmount { get; set; }
        public double totalCostAmount { get; set; }

        //public virtual User User { get; set; }

        //public int? UserId { get; set; }
        public virtual List<CostSnapshot> costSnapshots { get; set; }
        public virtual List<CostCategory> costCategories { get; set; }
    }
}
