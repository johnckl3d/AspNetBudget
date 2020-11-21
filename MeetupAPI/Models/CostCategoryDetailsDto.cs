using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class CostCategoryDetailsDto
    {
        public string costCategoryId { get; set; }
        public string name { get; set; }

        public double totalAmount { get; set; }
        public List<CostItemDto> costItems { get; set; }
    }
}

