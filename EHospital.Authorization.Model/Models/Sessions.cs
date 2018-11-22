﻿namespace EHospital.Authorization.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Sessions
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiredDate { get; set; }
    }
}
