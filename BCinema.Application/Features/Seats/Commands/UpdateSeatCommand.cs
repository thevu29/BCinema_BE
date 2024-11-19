using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Seats.Commands;

public class UpdateSeatCommand : IRequest<SeatDto>
{
    public Guid Id { get; set; }
    public Guid? SeatTypeId { get; set; }
    
    public class UpdateSeatCommandHandler(
        ISeatRepository seatRepository,
        ISeatTypeRepository seatTypeRepository,
        IMapper mapper): IRequestHandler<UpdateSeatCommand, SeatDto>
    {
        public async Task<SeatDto> Handle(UpdateSeatCommand request, CancellationToken cancellationToken)
        {
            var seat = await seatRepository.GetSeatByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Seat));

            if (request.SeatTypeId != null)
            {
                var seatType = await seatTypeRepository.GetByIdAsync(request.SeatTypeId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(SeatType));
                
                seat.SeatTypeId = seatType.Id;
            }
            
            await seatRepository.SaveChangesAsync(cancellationToken);
            
            return mapper.Map<SeatDto>(seat);
        }
    }
}