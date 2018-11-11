using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eHospital.Authorization
{
    public class Sessions
    {
        [Key]
        public int SessionId { get; set; }

        [Key]
        public int UserId { get; set; }

        public string Token { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiredDate { get; set; }

    }
}
