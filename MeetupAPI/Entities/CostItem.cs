﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    public class CostItem
    {
        public int Id { get; set; }
        public string Name { get; set; }


        [JsonIgnore]
        public virtual Budget Budget{ get; set; }
        public int BudgetId { get; set; }
        public double Amount { get; set; }
    }
}
