namespace EHospital.Authorization.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Secrets
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
