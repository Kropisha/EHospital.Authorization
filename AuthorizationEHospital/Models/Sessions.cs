using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eHospital.Authorization
{
    public class Sessions
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SessionId { get; set; }

      //  [ForeignKey("UsersData")]
        public int UserId { get; set; }

        public string Token { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiredDate { get; set; }

    }
}
