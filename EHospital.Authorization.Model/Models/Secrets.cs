namespace EHospital.Authorization.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Secrets
    {
        private const string Contains = "It must have at least one character at upper case, one at lower case, one digit and one special symbol.";

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Password should be entered.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long." + Contains, MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
