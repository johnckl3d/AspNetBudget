using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    public class CostCategory
    {
        public int Id { get; set; }
        public string name { get; set; }

        public string costCategoryId { get; set; }

        public double totalAmount { get; set; }

        //public virtual User User { get; set; }

        //public int? UserId { get; set; }
        public virtual List<CostItem> costItems { get; set; }
    }
}
