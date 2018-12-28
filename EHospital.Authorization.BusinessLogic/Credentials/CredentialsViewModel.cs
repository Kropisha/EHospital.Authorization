using FluentValidation.Attributes;

namespace EHospital.Authorization.BusinessLogic.Credentials
{
    /// <summary>
    /// Class for keeping users credentials
    /// </summary>
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserLogin { get; set; }

        public string Password { get; set; }
    }
}