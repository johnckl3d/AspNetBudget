using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MeetupAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MeetupAPI.Entities
{
    public class Budget
    {
        [Key]
        public string budgetId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double totalBudgetAmount { get; set; }
        public double totalCostAmount { get; set; }

        //public virtual User User { get; set; }

        public string userId { get; set; }


        public virtual List<CostCategory> costCategories { get; set; }

        public string createdBy { get; set; }


    }
}
