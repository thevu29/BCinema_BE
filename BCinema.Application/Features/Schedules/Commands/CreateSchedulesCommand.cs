using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands;

public class CreateSchedulesCommand : IRequest<SchedulesDto>
{
    public int MovieId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<TimeSpan> Times { get; set; } = new List<TimeSpan>();
    public string? Status { get; set; }

    public class CreateSchedulesCommandHandler(
        IScheduleRepository scheduleRepository,
        IRoomRepository roomRepository,
        IMovieFetchService movieFetchService,
        ISeatScheduleRepository seatScheduleRepository,
        ISeatRepository seatRepository,
        IMapper mapper) : IRequestHandler<CreateSchedulesCommand, SchedulesDto>
    {
        public async Task<SchedulesDto> Handle(CreateSchedulesCommand request, CancellationToken cancellationToken)
        {
            var movie = await movieFetchService.FetchMovieByIdAsync(request.MovieId) as MovieDto
                        ?? throw new NotFoundException("Movie");

            var room = await roomRepository.GetRoomByIdAsync(request.RoomId, cancellationToken)
                       ?? throw new NotFoundException("Room");
            
            var schedules = request.Times.Select(time =>
            {
                var schedule = mapper.Map<Schedule>(request);
                schedule.MovieName = movie.Title;
                schedule.Date = DateTime.SpecifyKind(request.Date.Date.Add(time), DateTimeKind.Utc);
                schedule.Runtime = movie.Runtime;
                
                return schedule;
            }).ToList();

            var seats = await seatRepository.GetSeatsByRoomIdAsync(room.Id, cancellationToken);

            var seatSchedules = schedules.SelectMany(schedule => 
                seats.Select(seat => new SeatSchedule 
                {
                    ScheduleId = schedule.Id,
                    SeatId = seat.Id,
                    Status = SeatSchedule.SeatScheduleStatus.Available
                })).ToList();

            await scheduleRepository.AddSchedulesAsync(schedules, cancellationToken);
            await seatScheduleRepository.AddSeatSchedulesAsync(seatSchedules, cancellationToken);
            
            await scheduleRepository.SaveChangesAsync(cancellationToken);

            var firstSchedule = schedules.First();

            var scheduleDto = mapper.Map<SchedulesDto>(firstSchedule);

            scheduleDto.Schedules = schedules.Select(schedule => new ScheduleDetailDto
            {
                Id = schedule.Id,
                Time = schedule.Date.TimeOfDay,
                Status = schedule.Status.ToString(),
            }).ToList();

            return scheduleDto;
        }
    }
}