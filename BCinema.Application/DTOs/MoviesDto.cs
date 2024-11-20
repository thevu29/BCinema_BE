using Newtonsoft.Json;

namespace BCinema.Application.DTOs
{
    public class MoviesDto
    {
        [JsonProperty("dates")]
        public DatesDto? Dates { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("results")]
        public IEnumerable<MovieDto> Results { get; set; } = default!;

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }

    public class DatesDto
    {
        [JsonProperty("maximum")]
        public string Maximum { get; set; } = default!;

        [JsonProperty("minimum")]
        public string Minimum { get; set; } = default!;
    }
}