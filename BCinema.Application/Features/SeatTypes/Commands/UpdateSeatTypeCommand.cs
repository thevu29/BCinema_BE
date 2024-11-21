using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Commands
{
    public class UpdateSeatTypeCommand : IRequest<SeatTypeDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = default!;
        public double Price { get; set; }

        public class UpdateSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            : IRequestHandler<UpdateSeatTypeCommand, SeatTypeDto>
        {
            public async Task<SeatTypeDto> Handle(
                UpdateSeatTypeCommand request,
                CancellationToken cancellationToken)
            {
                var seatType = await seatTypeRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(SeatType));
               
                mapper.Map(request, seatType);

                await seatTypeRepository.SaveChangesAsync(cancellationToken);

                return mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}
