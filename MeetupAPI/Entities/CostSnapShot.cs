using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Entities
{
    [NotMapped]
    public class CostSnapShot
    {
        public DateTime dateTime { get; set; }

        public double amount { get; set; }
    }
}

