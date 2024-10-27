using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace BCinema.Application.Features.Seats.Validator
{
    public class UpdateSeatCommandValidator : AbstractValidator<UpdateSeatCommand>
    {
        private readonly IValidator<DbContext> _context;

        public UpdateSeatCommandValidator()
        {
            RuleFor(x => x.Row)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.SeatStatus)
                .IsInEnum().WithMessage("{PropertyName} is not valid.");
            RuleFor(x => x.SeatTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }


    }
}
