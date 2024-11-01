using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Helpers;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Schedule.Queries;

public class GetSchedulesQuery : IRequest<PaginatedList<SchedulesDto>>
{
    public ScheduleQuery Query { get; set; } = default!;

    public class GetSchedulesQueryHandler : IRequestHandler<GetSchedulesQuery, PaginatedList<SchedulesDto>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMovieFetchService _movieFetchService;
        private readonly IMapper _mapper;
        
        public GetSchedulesQueryHandler(
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
        
        public async Task<PaginatedList<SchedulesDto>> Handle(GetSchedulesQuery request, CancellationToken cancellationToken)
        {
            var query = _scheduleRepository.GetSchedules();
            
            if (request.Query.Date.HasValue)
            {
                var utcDate = DateTime.SpecifyKind(request.Query.Date.Value, DateTimeKind.Utc);
                query = query.Where(s => s.Date.Date == utcDate);
            }
            if (request.Query.MovieId.HasValue)
            {
                var movie = await _movieFetchService.FetchMovieByIdAsync(request.Query.MovieId.Value) as MovieDto
                            ?? throw new NotFoundException("Movie");
                
                query = query.Where(s => s.MovieId == movie.Id);
            }
            if (request.Query.RoomId.HasValue)
            {
                var room = await _roomRepository.GetRoomByIdAsync(request.Query.RoomId.Value, cancellationToken) 
                           ?? throw new NotFoundException("Room");
                
                query = query.Where(s => s.RoomId == room.Id);
            }
            if (!string.IsNullOrEmpty(request.Query.Status))
            {
                if (!Enum.TryParse<Domain.Entities.Schedule.ScheduleStatus>(
                        request.Query.Status,
                        ignoreCase: true,
                        out var status))
                {
                    throw new NotFoundException("Status");
                }
                
                query = query.Where(s => s.Status == status);
            }

            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);
            
            var schedules = await query.ToListAsync(cancellationToken);
            
            var groupedSchedules = schedules
                .GroupBy(s => new { s.Date.Date, s.MovieId, s.RoomId })
                .Select(g =>
                {
                    var scheduleDto = _mapper.Map<SchedulesDto>(g.First());
                    
                    scheduleDto.Schedules = g.Select(s => new ScheduleDetailDto
                    {
                        Id = s.Id,
                        Time = s.Date.TimeOfDay,
                        Status = s.Status.ToString()
                    }).ToList();
                    return scheduleDto;
                })
                .ToList();
            
            var totalElements = groupedSchedules.Count;
            var paginatedList = groupedSchedules
                .Skip((request.Query.Page - 1) * request.Query.Size)
                .Take(request.Query.Size)
                .ToList();
            
            return new PaginatedList<SchedulesDto>(
                request.Query.Page,
                request.Query.Size,
                totalElements,
                paginatedList);
        }

        private static IQueryable<Domain.Entities.Schedule> ApplySorting(
            IQueryable<Domain.Entities.Schedule> query,
            string sortBy,
            string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                case "date":
                    return sortOrder.ToUpper().Equals("ASC")
                        ? query.OrderBy(s => s.Date)
                        : query.OrderByDescending(s => s.Date);
                default:
                    return query.OrderBy(s => s.Date);
            }
        }
    }
}