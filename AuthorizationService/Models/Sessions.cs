namespace eHospital.Authorization
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Sessions
    {
        [Key]
        public int SessionId { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiredDate { get; set; }

    }
}
