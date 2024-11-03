using BCinema.Application.Features.Schedules.Commands;
using FluentValidation;

namespace BCinema.Application.Features.Schedules.Validators;

public class UpdateScheduleCommandValidator : AbstractValidator<UpdateScheduleCommand>
{
    public UpdateScheduleCommandValidator()
    {
        RuleFor(x => x.Status)
            .Must(BeAValidStatus).WithMessage("Invalid status");
    }
    
    private static bool BeAValidStatus(string? status)
    {
        return status == null || Enum.TryParse<Domain.Entities.Schedule.ScheduleStatus>(status, ignoreCase: true, out _);
    }
}