using System;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Authorization.Models
{
    /// <summary>
    /// Db sessions
    /// </summary>
    public class Sessions
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(450, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Token { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiredDate { get; set; }
    }
}
