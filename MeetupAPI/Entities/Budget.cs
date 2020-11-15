using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string costCategoryId { get; set; }

        //public virtual User User { get; set; }

        //public int? UserId { get; set; }
        public virtual List<CostItem> costItems { get; set; }
    }
}
