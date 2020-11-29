using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class CostSnapShotDto
    {
        public CostSnapShotDto(DateTime dateTime, double amount)
        {
            this.dateTime = dateTime;
            this.amount = amount;
        }

        public DateTime dateTime { get; set; }

        public double amount { get; set; }

    }
}