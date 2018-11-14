using System.ComponentModel.DataAnnotations;

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
