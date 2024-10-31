using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Rooms.Queries;

public class RoomQuery : PaginationQuery
{
    public string? Name { get; set; }

    public RoomQuery() : base()
    {
    }
    
    public RoomQuery(string? name, int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
        Name = name;
    }
}