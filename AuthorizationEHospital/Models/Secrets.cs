using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Models
{
    public class Secrets
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
