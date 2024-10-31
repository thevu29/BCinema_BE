using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Utils;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Seats.Validators;

public class CreateSeatCommandValidator : AbstractValidator<CreateSeatCommand>
{
    public CreateSeatCommandValidator()
    {
        RuleFor(x => x.Row)
            .NotEmpty().WithMessage("Row is required");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Number is required")
            .GreaterThan(0).WithMessage("Number must be greater than 0");

        RuleFor(x => x.Status)
            .Must(BeAValidStatus).WithMessage("Invalid status");

        RuleFor(x => x.SeatTypeId)
            .NotEmpty().WithMessage("Seat type is required");

        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room is required");
    }

    private static bool BeAValidStatus(string? status)
    {
        return status == null || Enum.TryParse(typeof(Seat.SeatStatus), StringUtil.UppercaseFirstLetter(status), out _);
    }
}