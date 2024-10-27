using BCinema.Application.Features.SeatTypes.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;

namespace BCinema.Application.Features.SeatTypes.Validator
{
    public class UpdateSeatTypeCommandValidator : AbstractValidator<CreateSeatTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSeatTypeCommandValidator(){

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
