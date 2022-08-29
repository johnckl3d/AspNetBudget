﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class UserLogoutRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string IPAddress { get; set; }
    }
}

