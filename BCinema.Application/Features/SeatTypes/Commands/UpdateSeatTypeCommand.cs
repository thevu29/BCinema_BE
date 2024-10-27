using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;


namespace BCinema.Application.Features.SeatTypes.Commands
{
    public class UpdateSeatTypeCommand : IRequest<SeatTypeDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public DateTime CreateAt { get; set; }

        public class UpdateSeatTypeCommandHandler : IRequestHandler<UpdateSeatTypeCommand, SeatTypeDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public UpdateSeatTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(UpdateSeatTypeCommand request, CancellationToken cancellationToken)
            {
                var seatType = await _context.SeatTypes.FindAsync(request.Id)
                                ?? throw new NotFoundException(nameof(SeatType), request.Id);

                _mapper.Map(request, seatType);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}
