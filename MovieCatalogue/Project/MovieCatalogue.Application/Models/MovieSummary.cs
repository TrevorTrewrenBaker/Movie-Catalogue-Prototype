using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Models
{
    public class MovieSummary
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
        public string? BackdropUrl { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public double Popularity { get; set; }
        public IReadOnlyList<int> GenreIds { get; set; } = new List<int>();
    }
}
