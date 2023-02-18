using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeetupAPI.Models
{
    public class CostItemDto
    {
        public string costItemId { get; set; }
        [Required]
        [MinLength(5)]
        public string name { get; set; }
        [Required]
        public int type { get; set; } //0 = cost, //1 = income
        [Required]
        public double amount { get; set; }
        [Required]
        public DateTime dateTime { get; set; }

        public string description { get; set; }

    }
}

