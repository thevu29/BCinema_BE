using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Schedules.Queries;

public class GetAllSchedulesQuery : IRequest<IEnumerable<ScheduleDto>>
{
    public class GetAllSchedulesQueryHandler(
        IScheduleRepository scheduleRepository,
        IMapper mapper) : IRequestHandler<GetAllSchedulesQuery, IEnumerable<ScheduleDto>>
    {
        public async Task<IEnumerable<ScheduleDto>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
        {
            var schedules = await scheduleRepository.GetSchedulesAsync(cancellationToken);
            return mapper.Map<IEnumerable<ScheduleDto>>(schedules);
        }
    }
}