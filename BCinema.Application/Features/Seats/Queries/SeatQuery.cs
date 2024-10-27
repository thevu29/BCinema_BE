using BCinema.Application.Helpers;
using BCinema.Doman.Entities;

namespace BCinema.Application.Features.Seats.Queries
{
    public class SeatQuery : PaginationQuery
    {
        public string? Row { get; set; }   
        public int? Number { get; set; }   
        public Guid? RoomId { get; set; } 
        public SeatStatus? Status { get; set; }
        public SeatQuery() : base()
        {
        }

        public SeatQuery(int page, int size, string? row, int? number, Guid? roomId, SeatStatus? status) : base(page, size)
        {
            Row = row;
            Number = number;
            RoomId = roomId;
            Status = status;
        }
    }
}
