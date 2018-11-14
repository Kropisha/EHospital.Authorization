﻿namespace eHospital.Authorization.Controllers
{
    using FluentValidation.Attributes;

    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserLogin { get; set; }

        public string Password { get; set; }
    }
}