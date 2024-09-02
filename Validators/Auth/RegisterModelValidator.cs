using FluentValidation;
using TaskManager.Entities.DTO.Auth;

namespace TaskManager.Validators.Auth
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username required")
                .Length(2, 24).WithMessage("Username must be between 2 and 24 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email required")
                .EmailAddress().WithMessage("Wrong email type");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password required")
                .Length(8, 32).WithMessage("Password must be between 8 and 32 characters");
        }
    }
}
