using FluentValidation;
using TaskManager.Entities.DTO.Tasks;

namespace TaskManager.Validators.Tasks
{
    public class CreateTaskModelValidator : AbstractValidator<CreateTaskModel>
    {
        public CreateTaskModelValidator()
        {
            RuleFor(t => t.Title)
                .Length(2, 68).WithMessage("Title must be between 2 and 68 characters")
                .NotEmpty().WithMessage("Title required");

            RuleFor(t => t.Description)
                .MaximumLength(512).WithMessage("Descritpion max length 512 characters");

        }
    }
}
