using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int SeatRows { get; set; }
        public int SeatColumns { get; set; }

        public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
        {
            private readonly IRoomRepository _roomRepository;
            private readonly ISeatRepository _seatRepository;
            private readonly ISeatTypeRepository _seatTypeRepository;
            private readonly IMapper _mapper;

            public CreateRoomCommandHandler(
                IRoomRepository roomRepository,
                ISeatRepository seatRepository,
                ISeatTypeRepository seatTypeRepository,
                IMapper mapper) 
            {
                _roomRepository = roomRepository;
                _seatRepository = seatRepository;
                _seatTypeRepository = seatTypeRepository;
                _mapper = mapper;
            }

            public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
            {
                var room = _mapper.Map<Room>(request);

                await _roomRepository.AddRoomAsync(room, cancellationToken);
                await _roomRepository.SaveChangesAsync(cancellationToken);
                
                var seatType = await _seatTypeRepository
                    .GetByNameAsync("Regular", cancellationToken) 
                    ?? throw new NotFoundException("Seat type regular does not in database");

                foreach (var row in Enumerable.Range(0, request.SeatRows))
                {
                    var rowLabel = (char)('A' + row);

                    foreach (var column in Enumerable.Range(1, request.SeatColumns))
                    {
                        var seat = new Seat
                        {
                            Row = rowLabel.ToString(),
                            Number = column,
                            SeatTypeId = seatType.Id,
                            RoomId = room.Id,
                            Status = Seat.SeatStatus.Available
                        };
                        
                        await _seatRepository.AddSeatAsync(seat, cancellationToken);
                    }
                }

                await _seatRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoomDto>(room);
            }
        }
    }
}
