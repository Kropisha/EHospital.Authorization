using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Models
{
    [Table("UsersData")]
    public class UsersData : IdentityUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MaxLength(12)]
        public override string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public byte Gender { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public override string Email { get; set; }

        public bool IsDeleted { get; set; }
    }
}
