using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class BudgetDetailsDto
    {
        public string costCategoryId { get; set; }
        public string Name { get; set; }
        public List<CostItemDto> CostItems { get; set; }
    }
}

