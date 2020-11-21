using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    public class CostItem
    {
        public int Id { get; set; }
        public string costItemId { get; set; }
        public string name { get; set; }


        [JsonIgnore]
        public virtual CostCategory costCategory{ get; set; }
        public int costCategoryId { get; set; }
        public double amount { get; set; }
    }
}
