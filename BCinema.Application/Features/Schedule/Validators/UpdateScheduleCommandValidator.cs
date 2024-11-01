using BCinema.Application.Features.Schedule.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Schedule.Validators;

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