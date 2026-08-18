using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbMovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        public int Runtime { get; set; } // minutes

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = string.Empty; // "yyyy-MM-dd"

        public string Overview { get; set; } = string.Empty;

        public List<TmdbGenreDto> Genres { get; set; } = new();

        [JsonPropertyName("credits")]
        public TmdbCreditsDto? Credits { get; set; }
    }
}
