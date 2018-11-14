namespace eHospital.Authorization.Controllers
{
    using FluentValidation;

    public class CredentialsViewModelValidator : AbstractValidator<CredentialsViewModel>
    {
        public CredentialsViewModelValidator()
        {
            this.RuleFor(vm => vm.UserLogin).NotEmpty().WithMessage("User login cannot be empty");
            this.RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            this.RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters");
        }
    }
}