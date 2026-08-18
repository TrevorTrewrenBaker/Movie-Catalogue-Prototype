// MovieCatalogue.Infrastructure/TMDB/Dtos/TmdbMovieSummaryDto.cs
using MovieCatalogue.Domain.Entities;
using System.Text.Json.Serialization;

namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbMovieSummaryDto
    {
        public int Id { get; set; }

        // TV detail endpoints use "name"; movie detail endpoints use "title".
        // If you're only ever calling the movie endpoint, keep [JsonPropertyName("title")].
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        public string Overview { get; set; } = string.Empty;

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = string.Empty;

        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; } = string.Empty;

        // TV uses "first_air_date"; movie uses "release_date".
        [JsonPropertyName("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }

        public double Popularity { get; set; }

        public List<int> GenreIds { get; set; } = new();
    }

}