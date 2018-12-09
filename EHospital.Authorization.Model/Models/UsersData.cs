using System;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Authorization.Model.Models
{
    /// <summary>
    /// Db users data
    /// </summary>
    public class UsersData
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The field should not be empty.")]
        [StringLength(100, ErrorMessage = "No more than 50 symbols.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field should not be empty.")]
        [StringLength(100, ErrorMessage = "No more than 50 symbols.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field should not be empty.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MaxLength(12, ErrorMessage = "No more than 12 symbols. Without '+'.")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "No more than 50 symbols.")]
        public string Country { get; set; }

        [StringLength(100, ErrorMessage = "No more than 50 symbols.")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "No more than 50 symbols.")]
        public string Adress { get; set; }

        public byte Gender { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }
    }
}
