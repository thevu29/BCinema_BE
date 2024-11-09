using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;

namespace BCinema.Application.Features.Schedules.Queries;

public class GetScheduleByIdQuery  : IRequest<ScheduleDto>
{
    public Guid Id { get; init; }
    
    public class GetScheduleByIdQueryHandler(
        IScheduleRepository scheduleRepository,
        IMovieFetchService movieFetchService,
        IMapper mapper) : IRequestHandler<GetScheduleByIdQuery, ScheduleDto>
    {
        public async Task<ScheduleDto> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.Id, cancellationToken) 
                           ?? throw new NotFoundException("Schedule");

            var movie = await movieFetchService.FetchMovieByIdAsync(schedule.MovieId) as MovieDto 
                        ?? throw new NotFoundException("Movie");
            
            var scheduleDto = mapper.Map<ScheduleDto>(schedule);
            scheduleDto.MovieName = movie.Title;
            
            return scheduleDto;
        }
    }
}