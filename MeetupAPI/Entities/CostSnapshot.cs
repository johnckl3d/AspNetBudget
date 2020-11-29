using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    public class CostSnapshot
    {
        public DateTime dateTime { get; set; }

        public double totalCostAmount { get; set; }
    }
}
