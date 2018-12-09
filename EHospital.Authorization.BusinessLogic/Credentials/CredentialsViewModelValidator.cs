using FluentValidation;

namespace EHospital.Authorization.BusinessLogic.Credentials
{
    /// <summary>
    /// Set rules for credentials model
    /// </summary>
    public class CredentialsViewModelValidator : AbstractValidator<CredentialsViewModel>
    {
        public CredentialsViewModelValidator()
        {
            RuleFor(vm => vm.UserLogin).NotEmpty().WithMessage("User login cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 5 and 50 characters");
        }
    }
}