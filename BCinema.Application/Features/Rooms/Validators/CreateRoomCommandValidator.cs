using BCinema.Application.Features.Rooms.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Rooms.Validators
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        private readonly IRoomRepository _roomRepository;

        public CreateRoomCommandValidator(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Room name is required")
                .MustAsync(BeUniqueName).WithMessage("Room name already exists");

            RuleFor(x => x.SeatRows)
                .NotEmpty().WithMessage("Seat rows is required")
                .GreaterThan(0).WithMessage("Seat rows must be greater than 0");

            RuleFor(x => x.SeatColumns)
                .NotEmpty().WithMessage("Seat columns is required")
                .GreaterThan(0).WithMessage("Seat columns must be greater than 0");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return !await _roomRepository.AnyAsync(x => x.Name == name, cancellationToken);
        }
    }
}
