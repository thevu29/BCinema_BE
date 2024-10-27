using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.SeatTypes.Commands
{
    public class CreateSeatTypeCommand : IRequest<SeatTypeDto>
    {
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;

        public class CreateSeatTypeCommandHandler : IRequestHandler<CreateSeatTypeCommand, SeatTypeDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CreateSeatTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(CreateSeatTypeCommand request, CancellationToken cancellationToken)
            {
                var seatType = _mapper.Map<SeatType>(request);
                _context.SeatTypes.Add(seatType);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<SeatTypeDto>(seatType);
            }

        }

    }
}
