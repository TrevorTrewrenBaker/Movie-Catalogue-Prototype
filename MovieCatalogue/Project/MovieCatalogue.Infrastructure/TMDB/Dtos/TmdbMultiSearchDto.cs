using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbMultiSearchDto
    {
        public int Id { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; } = string.Empty;

        public string? Title { get; set; }   // movies
        public string? Name { get; set; }    // tv/person

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }
    }
}
