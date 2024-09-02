using FluentValidation;
using TaskManager.Entities.DTO.Auth;

namespace TaskManager.Validators.Auth
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login required")
                .Length(2, 12).WithMessage("Username must be between 2 and 40 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password required")
                .Length(8, 32).WithMessage("Password must be between 8 and 32 characters");
        }
    }
}
