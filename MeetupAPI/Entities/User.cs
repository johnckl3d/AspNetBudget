
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MeetupAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
namespace MeetupAPI.Entities
{
    public class User
    {
        [Key]
        public string userId { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string passwordHash { get; set; }
        public int roleId { get; set; }
        public Role Role  { get; set; }
    }
}
