﻿using System.ComponentModel.DataAnnotations.Schema;
using BCinema.Domain.Entities;

namespace BCinema.Domain.Entities
{
    [Table("MovieGenres")]
    public class MovieGenre : Base
    {
        public string MovieId { get; set; } = default!;
        public string GenreId { get; set; } = default!;

        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; } = default!;
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = default!;
    }
}
