using FluentValidation.Attributes;

namespace eHospital.Authorization.Controllers
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserLogin { get; set; }
        public string Password { get; set; }
    }
}