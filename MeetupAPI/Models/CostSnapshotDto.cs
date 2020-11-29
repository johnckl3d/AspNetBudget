using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class CostSnapshotDto
    {
        public DateTime dateTime { get; set; }

        public double totalCostAmount { get; set; }
    }
}