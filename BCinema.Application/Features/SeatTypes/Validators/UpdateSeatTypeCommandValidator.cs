using BCinema.Application.Features.SeatTypes.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.SeatTypes.Validators
{
    public class UpdateSeatTypeCommandValidator : AbstractValidator<UpdateSeatTypeCommand>
    {
        private readonly ISeatTypeRepository _seatTypeRepository;

        public UpdateSeatTypeCommandValidator(ISeatTypeRepository seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
                
            RuleFor(x => x.Name)
                .Must(desc => desc == null || !string.IsNullOrEmpty(desc))
                    .WithMessage("Name cannot be empty")
                .MustAsync(BeUniqueName).WithMessage("Seat type with the same name already exists");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }

        private async Task<bool> BeUniqueName(
            UpdateSeatTypeCommand command,
            string name,
            CancellationToken cancellationToken)
        {
            return !await _seatTypeRepository.AnyAsync(
                x => x.Name == name && command.Id != x.Id,
                cancellationToken);
        }
    }
}
