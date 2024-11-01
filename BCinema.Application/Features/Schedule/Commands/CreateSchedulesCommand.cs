using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;

namespace BCinema.Application.Features.Schedule.Commands;

public class CreateSchedulesCommand : IRequest<SchedulesDto>
{
    public int MovieId { get; set; }
    public Guid RoomId { get; set; }
    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    public IEnumerable<TimeSpan> Times { get; set; } = new List<TimeSpan>();
    public string? Status { get; set; }

    public class CreateSchedulesCommandHandler : IRequestHandler<CreateSchedulesCommand, SchedulesDto>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMovieFetchService _movieFetchService;
        private readonly IMapper _mapper;
        
        public CreateSchedulesCommandHandler(
            IScheduleRepository scheduleRepository,
            IRoomRepository roomRepository,
            IMovieFetchService movieFetchService,
            IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _movieFetchService = movieFetchService;
            _mapper = mapper;
        }

        public async Task<SchedulesDto> Handle(CreateSchedulesCommand request, CancellationToken cancellationToken)
        {
            var movie = await _movieFetchService.FetchMovieByIdAsync(request.MovieId)
                        ?? throw new NotFoundException("Movie");

            var room = await _roomRepository.GetRoomByIdAsync(request.RoomId, cancellationToken) 
                       ?? throw new NotFoundException("Room");

            var schedules = new List<Domain.Entities.Schedule>();
            
            foreach (var time in request.Times)
            {
                var schedule = _mapper.Map<Domain.Entities.Schedule>(request);
                schedule.Date = DateTime.SpecifyKind(request.Date.Date.Add(time), DateTimeKind.Utc);
                schedule.Runtime = movie.Runtime;
                schedules.Add(schedule);
            }
            
            await _scheduleRepository.AddSchedulesAsync(schedules, cancellationToken);
            await _scheduleRepository.SaveChangesAsync(cancellationToken);
            
            var firstSchedule = schedules.First();
            
            var scheduleDto = _mapper.Map<SchedulesDto>(firstSchedule);

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