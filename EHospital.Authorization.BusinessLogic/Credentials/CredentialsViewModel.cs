namespace EHospital.Authorization.BusinessLogic
{
    using FluentValidation.Attributes;

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