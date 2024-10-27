using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.Application.Features.SeatTypes.Commands
{
    public class DeleteSeatTypeCommand : IRequest<SeatTypeDto>
    {
        public Guid Id { get; set; }
        //public DateTime DeleteAt { get; set; }

        public class DeleteSeatTypeCommandHandler : IRequestHandler<DeleteSeatTypeCommand, SeatTypeDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public DeleteSeatTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(DeleteSeatTypeCommand request, CancellationToken cancellationToken)
            {

                var seatType = _context.SeatTypes.FirstOrDefault(x => x.Id == request.Id);
                if (seatType == null)
                {
                    throw new NotFoundException(nameof(SeatType), request.Id);
                }

                seatType.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<SeatTypeDto>(seatType);
            }
        }

    }
}
