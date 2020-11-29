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
        [Required]
        public double amount { get; set; }
        [Required]
        [MinLength(5)]
        public string name { get; set; }

        [Required]
        public DateTime dateTime { get; set; }

        public string description { get; set; }

    public string costItemId { get; set; }
    }
}

