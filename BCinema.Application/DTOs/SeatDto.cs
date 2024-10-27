using BCinema.Doman.Entities;

namespace BCinema.Application.DTOs
{
    public class SeatDto
    {
        public Guid Id { get; set; }
       
        public string Row { get; set; } = default!;

        public int Number { get; set; } = default!;
        public SeatStatus Status { get; set; } = SeatStatus.Available;
        public Guid SeatTypeId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CreateAt { get; set; }
    }

}
