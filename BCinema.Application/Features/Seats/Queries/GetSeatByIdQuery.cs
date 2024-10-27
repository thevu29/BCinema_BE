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

namespace BCinema.Application.Features.Seats.Queries
{

    public class GetSeatByIdQuery : IRequest<SeatDto>
    {
        public Guid Id { get; set; }

        public class GetSeatByIdQueryHandler : IRequestHandler<GetSeatByIdQuery, SeatDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSeatByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatDto> Handle(GetSeatByIdQuery request, CancellationToken cancellationToken)
            {
                var seat = await _context.Seats.FindAsync(request.Id)
                    ?? throw new NotFoundException(nameof(Seat), request.Id);

                if (seat.UpdateAt != null)
                {
                    throw new NotFoundException(nameof(Seat), request.Id);
                }

                return _mapper.Map<SeatDto>(seat);
            }
        }
    }
}
