using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class GetSeatTypeByIdQuery : IRequest<SeatTypeDto>
    {
        public Guid Id { get; set; }

        public class GetSeatTypeByIdQueryHandler : IRequestHandler<GetSeatTypeByIdQuery, SeatTypeDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSeatTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(GetSeatTypeByIdQuery request, CancellationToken cancellationToken)
            {
                var seatType = await _context.SeatTypes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(SeatType), request.Id);

                if (seatType.UpdateAt != null)
                {
                    throw new NotFoundException(nameof(SeatType), request.Id);
                }

                return _mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}