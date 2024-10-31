using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Utils;
using BCinema.Domain.Entities;
using FluentValidation;

namespace BCinema.Application.Features.Seats.Validators;

public class UpdateSeatCommandValidator : AbstractValidator<UpdateSeatCommand>
{
    public UpdateSeatCommandValidator()
    {
        RuleFor(x => x.Status)
            .Must(BeAValidStatus).WithMessage("Invalid status");

        RuleFor(x => x.SeatTypeId)
            .NotEmpty().WithMessage("Seat type is required");
    }

    private static bool BeAValidStatus(string? status)
    {
        return status == null || Enum.TryParse(typeof(Seat.SeatStatus), StringUtil.UppercaseFirstLetter(status), out _);
    }
}