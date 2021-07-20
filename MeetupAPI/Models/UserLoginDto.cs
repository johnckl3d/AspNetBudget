﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class UserLoginDto
    {
        [Required]
        public string Userid { get; set; }

        [Required]
        public string Password { get; set; }
        public string IPAddress { get; set; }
    }
}
