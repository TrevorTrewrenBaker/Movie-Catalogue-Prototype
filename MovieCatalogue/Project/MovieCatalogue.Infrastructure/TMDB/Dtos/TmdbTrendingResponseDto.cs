using System.Text.Json.Serialization; 
namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbTrendingResponseDto
    {
        public int Page { get; set; }
        public List<TmdbMovieSummaryDto> Results { get; set; } = new();

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}