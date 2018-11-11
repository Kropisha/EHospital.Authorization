using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Models
{
    public class Logins
    {
        [Key]
        public int LoginId { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Login { get; set; }

        [Key]
        public int RoleId { get; set; }
    }
}
