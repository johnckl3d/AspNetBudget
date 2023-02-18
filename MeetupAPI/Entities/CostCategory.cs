using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeetupAPI.Entities
{
    public class CostCategory
    {

        [Key]
        public string costCategoryId { get; set; }
        public string name { get; set; }


        public double totalAmount { get; set; }

        //public virtual User User { get; set; }

        //public int? UserId { get; set; }
        [JsonIgnore]
        public virtual Budget budget { get; set; }
        public string budgetId { get; set; }
        public virtual List<Item> costItems { get; set; }
    }
}
