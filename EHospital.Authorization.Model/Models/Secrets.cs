using System.ComponentModel.DataAnnotations;

namespace EHospital.Authorization.Model.Models
{
    /// <summary>
    /// Db secrets
    /// </summary>
    public class Secrets
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Password should be entered.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
