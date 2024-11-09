using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Schedules.Queries;

public class GetSchedulesQuery : IRequest<PaginatedList<SchedulesDto>>
{
    public ScheduleQuery Query { get; init; } = default!;

    public class GetSchedulesQueryHandler(
        IScheduleRepository scheduleRepository,
        IRoomRepository roomRepository,
        IMovieFetchService movieFetchService,
        IMapper mapper)
        : IRequestHandler<GetSchedulesQuery, PaginatedList<SchedulesDto>>
    {
        public async Task<PaginatedList<SchedulesDto>> Handle(GetSchedulesQuery request, CancellationToken cancellationToken)
        {
            var query = scheduleRepository.GetSchedules();
            
            if (!string.IsNullOrEmpty(request.Query.Date))
            {
                query = query.FilterByDate(request.Query.Date, s => s.Date);
            }
            if (request.Query.MovieId.HasValue)
            {
                var movie = await movieFetchService.FetchMovieByIdAsync(request.Query.MovieId.Value) as MovieDto
                            ?? throw new NotFoundException("Movie");
                
                query = query.Where(s => s.MovieId == movie.Id);
            }
            if (request.Query.RoomId.HasValue)
            {
                var room = await roomRepository.GetRoomByIdAsync(request.Query.RoomId.Value, cancellationToken) 
                           ?? throw new NotFoundException("Room");
                
                query = query.Where(s => s.RoomId == room.Id);
            }
            if (!string.IsNullOrEmpty(request.Query.Status))
            {
                if (!Enum.TryParse<Schedule.ScheduleStatus>(
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
                    var scheduleDto = mapper.Map<SchedulesDto>(g.First());
                    var movie = movieFetchService.FetchMovieByIdAsync(scheduleDto.MovieId).Result as MovieDto
                                ?? throw new NotFoundException("Movie");
                    
                    scheduleDto.MovieName = movie.Title;
                    
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

        private static IQueryable<Schedule> ApplySorting(IQueryable<Schedule> query, string sortBy, string sortOrder)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Schedule.Date)
            };
            
            if (string.IsNullOrEmpty(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                return query.OrderByDescending(s => s.Date);
            }

            return query.ApplyDynamicSorting(sortBy, sortOrder);
        }
    }
}