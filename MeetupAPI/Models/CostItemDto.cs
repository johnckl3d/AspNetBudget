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
        public double Amount { get; set; }
        [Required]
        [MinLength(5)]
        public string Name { get; set; }
    }
}

