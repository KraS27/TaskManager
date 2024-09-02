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
                .Length(8, 32).WithMessage("Password must be between 8 and 32 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }
    }
}
