using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;


namespace BCinema.Application.Features.Seats.Validator
{
    public class CreateSeatCommandValidator : AbstractValidator<CreateSeatCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateSeatCommandValidator( )
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
