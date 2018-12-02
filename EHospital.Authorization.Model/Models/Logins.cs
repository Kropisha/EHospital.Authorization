namespace EHospital.Authorization.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Db logins
    /// </summary>
    public class Logins
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Login { get; set; }

        public int RoleId { get; set; }
    }
}
