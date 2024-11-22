using BCinema.Application.Exceptions;
using BCinema.Application.Features.Schedules.Commands;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using FluentValidation;

namespace BCinema.Application.Features.Schedules.Validators;

public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IMovieFetchService _movieFetchService;
    
    public CreateScheduleCommandValidator(IScheduleRepository scheduleRepository, IMovieFetchService movieFetchService)
    {
        _scheduleRepository = scheduleRepository;
        _movieFetchService = movieFetchService;
        
        RuleFor(x => x.MovieId).GreaterThan(0).WithMessage("MovieId is required");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("RoomId is required");
        
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required")
            .Must(BeAValidDate).WithMessage("Date must be a future date")
            .Must(BeWithinWorkingHours).WithMessage("Schedule must be within working hours (8:00 - 23:00)");
        
        RuleFor(x => x.Status)
            .Must(BeAValidStatus).WithMessage("Invalid status");
        
        RuleFor(x => x).MustAsync(NoOverlappingSchedules)
            .WithMessage("Schedules cannot overlap with existing schedules");
    }
    
    private async Task<bool> NoOverlappingSchedules(CreateScheduleCommand command, CancellationToken cancellationToken)
    {
        var movie = await _movieFetchService.FetchMovieByIdAsync(command.MovieId)
                    ?? throw new NotFoundException("Movie");

        var schedules = await _scheduleRepository
            .GetSchedulesByRoomAndDateAsync(command.RoomId, DateOnly.FromDateTime(command.Date), cancellationToken);
        
        if (command.Date is { Hour: 0, Minute: 0 })
        {
            throw new ValidationException("Time must be specified");
        }
        
        var newScheduleStart = command.Date;
        var newScheduleEnd = newScheduleStart.AddMinutes(movie.Runtime);
    
        return !schedules.Any(schedule =>
        {
            var existingScheduleStart = schedule.Date;
            var existingScheduleEnd = existingScheduleStart.AddMinutes(schedule.Runtime);
            return newScheduleStart < existingScheduleEnd && newScheduleEnd > existingScheduleStart;
        });
    }
    
    private static bool BeAValidStatus(string? status)
    {
        return status == null || Enum.TryParse<Schedule.ScheduleStatus>(status, ignoreCase: true, out _);
    }
    
    private bool BeAValidDate(DateTime date)
    {
        return date.Date >= DateTime.Today;
    }

    private bool BeWithinWorkingHours(DateTime date)
    {
        var time = TimeOnly.FromDateTime(date);
        var workingStart = new TimeOnly(8, 0);
        var workingEnd = new TimeOnly(23, 0);

        return time >= workingStart && time <= workingEnd;
    }
}