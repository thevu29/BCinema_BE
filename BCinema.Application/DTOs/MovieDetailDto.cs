using Newtonsoft.Json;

namespace BCinema.Application.DTOs;

public class MovieDetailDto : MovieDto
{
    public new IEnumerable<Genre> Genres { get; set; } = default!;
}

public class Genre
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}