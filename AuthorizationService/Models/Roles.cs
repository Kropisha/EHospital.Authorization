using System.ComponentModel.DataAnnotations;

namespace eHospital.Authorization.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        
        public string Title { get; set; }

    }
}
