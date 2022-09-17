using System;
using System.Collections.Generic;

namespace MeetupAPI.Models
{
    public class CreateBudgetRequestDto
    {
        public string name { get; set; }
        public string description { get; set; }
        public double totalBudgetAmount { get; set; }
    }
    
}

