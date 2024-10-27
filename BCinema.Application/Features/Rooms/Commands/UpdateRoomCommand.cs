using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Rooms.Commands;

public class UpdateRoomCommand : IRequest<RoomDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public UpdateRoomCommandHandler(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Room));
            
            _mapper.Map(request, room);

            await _roomRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomDto>(room);
        }
    }
}