using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands
{
    public class CreateScheduleCommand : IRequest<ScheduleDto>
    {
        public Guid RoomId { get; set; }
        public string MovieId { get; set; } = default!; 
        public DateTime Date { get; set; } = DateTime.Now;
        public ScheduleStatus Status { get; set; } = ScheduleStatus.ComingSoon;

        public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, ScheduleDto>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CreateScheduleCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ScheduleDto> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
            {
                var room = await _context.Rooms.FindAsync(request.RoomId)
                    ?? throw new NotFoundException(nameof(Room), request.RoomId);
                var movie = await _context.Movies.FindAsync(request.MovieId)
                    ?? throw new NotFoundException(nameof(Movie), request.MovieId);

                var schedule = _mapper.Map<Schedule>(request);
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<ScheduleDto>(schedule);
            }
        }
    }
}
