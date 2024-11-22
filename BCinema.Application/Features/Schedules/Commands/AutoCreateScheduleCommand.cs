using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands;

public class AutoCreateScheduleCommand : IRequest<IEnumerable<SchedulesDto>>
{
    public int MovieId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime[] Dates { get; set; } = default!;
    public int Amount { get; set; }
    public string? Status { get; set; }
    
    public class AutoCreateScheduleCommandHandler(
        IScheduleRepository scheduleRepository,
        IRoomRepository roomRepository,
        IMovieFetchService movieFetchService,
        ISeatScheduleRepository seatScheduleRepository,
        ISeatRepository seatRepository,
        IMapper mapper) : IRequestHandler<AutoCreateScheduleCommand, IEnumerable<SchedulesDto>>
    {
        public async Task<IEnumerable<SchedulesDto>> Handle(AutoCreateScheduleCommand request, CancellationToken cancellationToken)
        {
            var movie = await movieFetchService.FetchMovieByIdAsync(request.MovieId) as MovieDto
                        ?? throw new NotFoundException("Movie");
            
            var room = await roomRepository.GetRoomByIdAsync(request.RoomId, cancellationToken)
                       ?? throw new NotFoundException("Room");
            
            var result = new List<SchedulesDto>();
            var workingStart = new TimeOnly(8, 0);
            var workingEnd = new TimeOnly(23, 0);
            var schedulesPerDay = (int)Math.Ceiling((double)request.Amount / request.Dates.Length);
            var remainingSchedules = request.Amount;

            foreach (var date in request.Dates)
            {
                var dateSchedulesDto = new SchedulesDto
                {
                    Date = DateOnly.FromDateTime(date),
                    Runtime = movie.Runtime,
                    MovieId = movie.Id,
                    MovieName = movie.Title,
                    RoomId = room.Id,
                    RoomName = room.Name,
                    Schedules = new List<ScheduleDetailDto>()
                };

                var existingSchedules = await scheduleRepository.GetSchedulesByRoomAndDateAsync(
                    room.Id,
                    DateOnly.FromDateTime(date),
                    cancellationToken);

                var busySlots = existingSchedules
                    .Select(s => new TimeSlot
                    {
                        Start = TimeOnly.FromDateTime(s.Date),
                        End = TimeOnly.FromDateTime(s.Date).AddMinutes(s.Runtime)
                    })
                    .ToList();

                var schedulesToCreate = Math.Min(schedulesPerDay, remainingSchedules);
        
                var availableSlots = GetOptimalTimeSlots(
                    workingStart,
                    workingEnd,
                    busySlots,
                    TimeSpan.FromMinutes(movie.Runtime),
                    schedulesToCreate);

                if (availableSlots.Count < schedulesToCreate)
                {
                    throw new BadRequestException(
                        $"Cannot create {schedulesToCreate} schedules for date {date:d}. " +
                        $"Only {availableSlots.Count} slots are available.");
                }
                
                foreach (var slot in availableSlots)
                {
                    var scheduleDate = date.Date + slot.Start.ToTimeSpan();
                    
                    var schedule = new Schedule
                    {
                        MovieId = request.MovieId,
                        MovieName = movie.Title,
                        RoomId = request.RoomId,
                        Date = DateTime.SpecifyKind(scheduleDate, DateTimeKind.Utc),
                        Runtime = movie.Runtime,
                    };

                    if (Enum.TryParse<Schedule.ScheduleStatus>(request.Status, true, out var status))
                    {
                        schedule.Status = status;
                    }
                    else
                    {
                        throw new BadRequestException($"Invalid status value: {request.Status}");
                    }

                    var createdSchedule = await scheduleRepository.AddScheduleAsync(schedule, cancellationToken);
                    
                    var scheduleDetailDto = mapper.Map<ScheduleDetailDto>(createdSchedule);
                    scheduleDetailDto.Time = TimeOnly.FromDateTime(createdSchedule.Date).ToString("HH:mm");
                    
                    ((List<ScheduleDetailDto>)dateSchedulesDto.Schedules).Add(scheduleDetailDto);
                    
                    var seats = await seatRepository.GetSeatsByRoomIdAsync(request.RoomId, cancellationToken);
                    var seatSchedules = seats.Select(seat => new SeatSchedule 
                    {
                        ScheduleId = createdSchedule.Id,
                        SeatId = seat.Id,
                        Status = SeatSchedule.SeatScheduleStatus.Available
                    }).ToList();
                    
                    await seatScheduleRepository.AddSeatSchedulesAsync(seatSchedules, cancellationToken);
                }

                result.Add(dateSchedulesDto);
                remainingSchedules -= dateSchedulesDto.Schedules.Count();
            }

            return result;
        }
    }
    
    private class TimeSlot
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }

    private static List<TimeSlot> GetOptimalTimeSlots(
        TimeOnly workingStart,
        TimeOnly workingEnd,
        List<TimeSlot> busySlots,
        TimeSpan movieDuration,
        int requiredSlots)
    {
        var result = new List<TimeSlot>();
        var currentTime = RoundToNearestInterval(workingStart);
    
        while (currentTime.AddMinutes(movieDuration.TotalMinutes) <= workingEnd && result.Count < requiredSlots)
        {
            var potentialSlot = new TimeSlot
            {
                Start = currentTime,
                End = currentTime.AddMinutes(movieDuration.TotalMinutes)
            };

            if (IsSlotAvailable(potentialSlot, busySlots))
            {
                result.Add(potentialSlot);
                currentTime = RoundToNearestInterval(currentTime.AddMinutes(movieDuration.TotalMinutes + 15));
            }
            else
            {
                currentTime = RoundToNearestInterval(currentTime.AddMinutes(15));
            }
        }

        return result;
    }
    
    private static TimeOnly RoundToNearestInterval(TimeOnly time)
    {
        var totalMinutes = time.Hour * 60 + time.Minute;
        
        if (totalMinutes % 30 == 0) return time;
        if (totalMinutes % 20 == 0) return time;
        if (totalMinutes % 15 == 0) return time;
        
        foreach (var interval in new[] { 15, 20, 30 })
        {
            var nextInterval = ((totalMinutes + interval - 1) / interval) * interval;
            if (nextInterval > totalMinutes)
            {
                return new TimeOnly(nextInterval / 60, nextInterval % 60);
            }
        }
        
        var next15Min = ((totalMinutes + 14) / 15) * 15;
        return new TimeOnly(next15Min / 60, next15Min % 60);
    }

    private static bool IsSlotAvailable(TimeSlot newSlot, List<TimeSlot> busySlots)
    {
        foreach (var busySlot in busySlots)
        {
            if (newSlot.Start < busySlot.End && busySlot.Start < newSlot.End)
            {
                return false;
            }
        }
        return true;
    }
}