﻿namespace BCinema.Application.DTOs
{
    public class SeatDto
    {
        public Guid Id { get; set; }
        public string Row { get; set; } = default!;
        public int Number { get; set; }
        public string SeatType { get; set; } = default!;
        public double Price { get; set; }
        public string Room { get; set; } = default!;
    }
}
