namespace EHospital.Authorization.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Roles
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
