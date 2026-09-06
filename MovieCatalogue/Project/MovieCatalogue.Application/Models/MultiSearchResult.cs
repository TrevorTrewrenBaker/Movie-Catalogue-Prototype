using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Models
{
    public class MultiSearchResult
    {
        public int Id { get; set; }
        public string MediaType { get; set; } = string.Empty; // "movie", "tv", "person"
        public string DisplayName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
