using System.ComponentModel.DataAnnotations;

namespace EHospital.Authorization.Models
{
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

        public string RegisterKey { get; set; }

        public string Status { get; set; }
    }
}
