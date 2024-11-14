using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Rooms.Queries;

public class GetRoomsQuery : IRequest<PaginatedList<RoomDto>>
{
    public RoomQuery Query { get; init; } = default!;

    public class GetRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
        : IRequestHandler<GetRoomsQuery, PaginatedList<RoomDto>>
    {
        public async Task<PaginatedList<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var query = roomRepository.GetRooms();
            
            if (!string.IsNullOrWhiteSpace(request.Query.Search))
            {
                var searchTerm = request.Query.Search.Trim().ToLower();
                query = query.Where(r => EF.Functions.Like(r.Name.ToLower(), $"%{searchTerm}%"));
            }
            
            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);
            
            var rooms = await PaginatedList<Room>
                .ToPageList(query, request.Query.Page, request.Query.Size);
            
            var roomsDto = mapper.Map<IEnumerable<RoomDto>>(rooms.Data);
            
            return new PaginatedList<RoomDto>(
                rooms.Page,
                rooms.Size,
                rooms.TotalElements,
                roomsDto);
        }
        
        private static IQueryable<Room> ApplySorting(IQueryable<Room> query, string sortBy, string sortOrder)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Room.Name),
                nameof(Room.CreateAt)
            };
            
            if (string.IsNullOrEmpty(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                return query.OrderByDescending(r => r.CreateAt);
            }

            return query.ApplyDynamicSorting(sortBy, sortOrder);            
        }
    }
}