using Newtonsoft.Json;

namespace BCinema.Application.DTOs;

public class MovieGenreDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}