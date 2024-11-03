using BCinema.Application.Exceptions;
using BCinema.Application.Features.Schedules.Commands;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using FluentValidation;

namespace BCinema.Application.Features.Schedules.Validators;

public class CreatSchedulesCommandValidator : AbstractValidator<CreateSchedulesCommand>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IMovieFetchService _movieFetchService;

    public CreatSchedulesCommandValidator(IScheduleRepository scheduleRepository, IMovieFetchService movieFetchService)
    {
        _scheduleRepository = scheduleRepository;
        _movieFetchService = movieFetchService;
        
        RuleFor(x => x.MovieId).GreaterThan(0).WithMessage("MovieId is required");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("RoomId is required");
        RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required");
        
        RuleFor(x => x.Times)
            .NotEmpty().WithMessage("At least one time is required")
            .Must(HaveUniqueTimes).WithMessage("Times must not have duplicates");
        
        RuleFor(x => x.Status)
            .Must(BeAValidStatus).WithMessage("Invalid status");
        
        RuleFor(x => x).MustAsync(NoOverlappingSchedules)
            .WithMessage("Schedules cannot overlap with existing schedules");
    }

    private async Task<bool> NoOverlappingSchedules(CreateSchedulesCommand command, CancellationToken cancellationToken)
    {
        var movie = await _movieFetchService.FetchMovieByIdAsync(command.MovieId) 
                    ?? throw new NotFoundException("Movie");
        
        var schedules = await _scheduleRepository
            .GetSchedulesByRoomAndDateAsync(command.RoomId, command.Date, cancellationToken);

        return !(from schedule in schedules from time in command.Times 
            let newScheduleStart = DateTime.SpecifyKind(command.Date.Date.Add(time), DateTimeKind.Utc)
            let newScheduleEnd = newScheduleStart.AddMinutes(movie.Runtime) 
            let existingScheduleStart = schedule.Date 
            let existingScheduleEnd = existingScheduleStart.AddMinutes(schedule.Runtime) 
            where newScheduleStart < existingScheduleEnd && newScheduleEnd > existingScheduleStart 
            select newScheduleStart).Any();
    }
    
    private static bool HaveUniqueTimes(IEnumerable<TimeSpan> times)
    {
        var timesList = times.ToList();
        return timesList.Distinct().Count() == timesList.Count;
    }
    
    private static bool BeAValidStatus(string? status)
    {
        return status == null || Enum.TryParse<Schedule.ScheduleStatus>(status, ignoreCase: true, out _);
    }
}