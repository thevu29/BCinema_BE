using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Commands
{
    public class CreateSeatTypeCommand : IRequest<SeatTypeDto>
    {
        public string Name { get; set; } = default!;
        public double Price { get; set; }

        public class CreateSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            : IRequestHandler<CreateSeatTypeCommand, SeatTypeDto>
        {
            public async Task<SeatTypeDto> Handle(CreateSeatTypeCommand request, CancellationToken cancellationToken)
            {
                var seatType = mapper.Map<SeatType>(request);

                await seatTypeRepository.AddAsync(seatType, cancellationToken);
                await seatTypeRepository.SaveChangesAsync(cancellationToken);

                return mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}
