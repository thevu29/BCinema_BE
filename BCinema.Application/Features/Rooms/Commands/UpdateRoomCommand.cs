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
    
    public class UpdateRoomCommandHandler(IRoomRepository roomRepository, IMapper mapper)
        : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetRoomByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Room));
            
            mapper.Map(request, room);

            await roomRepository.SaveChangesAsync(cancellationToken);

            return mapper.Map<RoomDto>(room);
        }
    }
}