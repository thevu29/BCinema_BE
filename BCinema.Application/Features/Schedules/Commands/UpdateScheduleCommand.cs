using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands
{
    public class UpdateScheduleCommand : IRequest<ScheduleDto>
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string MovieId { get; set; } = default!;
        public DateTime Date { get; set; } = DateTime.Now;
        public ScheduleStatus Status { get; set; } = ScheduleStatus.ComingSoon;

        public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, ScheduleDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public UpdateScheduleCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ScheduleDto> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
            {
                var schedule = await _context.Schedules.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Schedule), request.Id); 

                _mapper.Map(request, schedule);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<ScheduleDto>(schedule);
            }
        }
    }
}
