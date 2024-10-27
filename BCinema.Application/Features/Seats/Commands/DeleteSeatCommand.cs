using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.Seats.Commands
{
    public class DeleteSeatCommand : IRequest<SeatDto>
    {
        public Guid Id { get; set; }
        public DateTime DeleteAt { get; set; }

        public class DeleteSeatCommandHandler : IRequestHandler<DeleteSeatCommand, SeatDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public DeleteSeatCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatDto> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
            {
                var seat = await _context.Seats.FindAsync(request.Id);
                if (seat == null)
                {
                    throw new NotFoundException(nameof(Seat), request.Id);
                }

                seat.UpdateAt = DateTime.Now;
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<SeatDto>(seat);
            }
        }
    }
}
