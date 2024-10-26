using BCinema.Application.Features.Users.Commands;
using FluentValidation;

namespace BCinema.Application.Features.Users.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {        
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty")
                .When(x => x.Name != null);
        }
    }
}
