using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands
{
    public class DeleteScheduleCommand : IRequest<ScheduleDto>
    {
        public Guid Id { get; set; }
        public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, ScheduleDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public DeleteScheduleCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ScheduleDto> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
            {
                var schedule = await _context.Schedules.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Schedule), request.Id);

                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<ScheduleDto>(schedule);
            }
        }
    }
}
