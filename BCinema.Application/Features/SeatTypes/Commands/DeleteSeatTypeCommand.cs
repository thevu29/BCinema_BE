using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Commands;

public class DeleteSeatTypeCommand : IRequest
{
    public Guid Id { get; init; }

    public class DeleteSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository)
        : IRequestHandler<DeleteSeatTypeCommand>
    {
        public async Task Handle(DeleteSeatTypeCommand request, CancellationToken cancellationToken)
        {
            var seatType = await seatTypeRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Seat type");

            seatType.DeleteAt = DateTime.UtcNow;
            
            await seatTypeRepository.SaveChangesAsync(cancellationToken);
        }
    }
}