using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Utils;
using BCinema.Domain.Entities;
using FluentValidation;

namespace BCinema.Application.Features.Seats.Validators;

public class UpdateSeatCommandValidator : AbstractValidator<UpdateSeatCommand>
{
    public UpdateSeatCommandValidator()
    {
        RuleFor(x => x.SeatTypeId)
            .NotEmpty().WithMessage("Seat type is required");
    }
}