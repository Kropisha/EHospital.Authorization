namespace eHospital.Authorization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Logins
    {
       // [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginId { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Login { get; set; }

        // [ForeignKey("FK_UsersLogin_Roles")]
        [Key]
        public int RoleId { get; set; }
    }
}
