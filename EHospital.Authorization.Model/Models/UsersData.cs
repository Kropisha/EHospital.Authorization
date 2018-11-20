namespace EHospital.Authorization.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UsersData")]
    public class UsersData
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
        public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Adress { get; set; }

        public byte Gender { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }
    }
}
