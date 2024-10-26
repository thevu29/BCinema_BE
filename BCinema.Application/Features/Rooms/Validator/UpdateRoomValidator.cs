using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Rooms.Validator
{
    public class UpdateRoomValidator : AbstractValidator<UpdateRoomCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRoomValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(BeUniqueName).WithMessage("{PropertyName} already exists.");
        }
        private async Task<bool> BeUniqueName(UpdateRoomCommand command, string name, CancellationToken cancellationToken)
        {
            return !await _context.Rooms.AnyAsync(x => x.Name == name && x.Id != command.Id, cancellationToken);
        }
    }
}
