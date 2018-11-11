using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        
        public string Title { get; set; }

    }
}
