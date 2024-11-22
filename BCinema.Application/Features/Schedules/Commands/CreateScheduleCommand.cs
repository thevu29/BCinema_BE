using System.Text.Json.Serialization;
using AutoMapper;
using BCinema.Application.Converters;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands;

public class CreateScheduleCommand : IRequest<ScheduleDto>
{
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public Guid RoomId { get; set; }
    public string? Status { get; set; }

    public class CreateScheduleCommandHandler(
        IScheduleRepository scheduleRepository,
        IRoomRepository roomRepository,
        ISeatRepository seatRepository,
        ISeatScheduleRepository seatScheduleRepository,
        IMovieFetchService movieFetchService,
        IMapper mapper) : IRequestHandler<CreateScheduleCommand, ScheduleDto>
    {
        public async Task<ScheduleDto> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            var movie = await movieFetchService.FetchMovieByIdAsync(request.MovieId) as MovieDto
                        ?? throw new NotFoundException("Movie");

            var room = await roomRepository.GetRoomByIdAsync(request.RoomId, cancellationToken)
                       ?? throw new NotFoundException("Room");

            var schedule = new Schedule()
            {
                Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
                MovieId = movie.Id,
                MovieName = movie.Title,
                RoomId = room.Id,
                Runtime = movie.Runtime,
                Status = Enum.TryParse<Schedule.ScheduleStatus>(request.Status, true, out var status)
                    ? status
                    : Schedule.ScheduleStatus.Available
            };

            var createdSchedule = await scheduleRepository.AddScheduleAsync(schedule, cancellationToken);

            var seats = await seatRepository.GetSeatsByRoomIdAsync(request.RoomId, cancellationToken);
            var seatSchedules = seats.Select(seat => new SeatSchedule 
            {
                ScheduleId = createdSchedule.Id,
                SeatId = seat.Id,
                Status = SeatSchedule.SeatScheduleStatus.Available
            }).ToList();
            
            await seatScheduleRepository.AddSeatSchedulesAsync(seatSchedules, cancellationToken);
            
            return mapper.Map<ScheduleDto>(schedule);
        }
    }
}