using BCinema.Application.Features.Roles.Commands;
using FluentValidation;

namespace BCinema.Application.Features.Roles.Validator
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
