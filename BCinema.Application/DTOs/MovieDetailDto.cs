using Newtonsoft.Json;

namespace BCinema.Application.DTOs;

public class MovieDetailDto : MovieDto
{
    public new IEnumerable<MovieGenreDto> Genres { get; set; } = default!;
}