using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    public class CostItem
    {
        [Key]
        public string costItemId { get; set; }
        public string name { get; set; }


        [JsonIgnore]
        public virtual CostCategory costCategory{ get; set; }
        public string costCategoryId { get; set; }
        public double amount { get; set; }

        public DateTime dateTime { get; set; }

        public string description { get; set; }
    }
}
