using BCinema.Application.Features.SeatTypes.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.SeatTypes.Validators
{
    public class CreateSeatTypeCommandValidator : AbstractValidator<CreateSeatTypeCommand>
    {
        private readonly ISeatTypeRepository _seatTypeRepository;

        public CreateSeatTypeCommandValidator(ISeatTypeRepository seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Seat type name is required")
                .MustAsync(BeUniqueName).WithMessage("Seat type name already exists");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return !await _seatTypeRepository.AnyAsync(x => x.Name != name, cancellationToken);
        }
    }
}
