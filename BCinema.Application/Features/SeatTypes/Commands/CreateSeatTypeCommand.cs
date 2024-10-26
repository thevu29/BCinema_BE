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

        public class CreateSeatTypeCommandHandler : IRequestHandler<CreateSeatTypeCommand, SeatTypeDto>
        {
            private readonly ISeatTypeRepository _seatTypeRepository;
            private readonly IMapper _mapper;

            public CreateSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            {
                _seatTypeRepository = seatTypeRepository;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(
                CreateSeatTypeCommand request,
                CancellationToken cancellationToken)
            {
                var seatType = _mapper.Map<SeatType>(request);

                await _seatTypeRepository.AddAsync(seatType, cancellationToken);
                await _seatTypeRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}
