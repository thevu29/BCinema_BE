using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Rooms.Validator
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateRoomCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Room name is required.")
                .MustAsync(BeUniqueName).WithMessage("Room name already exists");

            RuleFor(x => x.SeatRows)
                .NotEmpty().WithMessage("Seat rows is required.")
                .GreaterThan(0).WithMessage("Seat rows must be greater than 0");

            RuleFor(x => x.SeatColumns)
                .NotEmpty().WithMessage("Seat columns is required.")
                .GreaterThan(0).WithMessage("Seat columns must be greater than 0");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _context.Rooms.AllAsync(x => x.Name != name, cancellationToken);
        }
    }
}
