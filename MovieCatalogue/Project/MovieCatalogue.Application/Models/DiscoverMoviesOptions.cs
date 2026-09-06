using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Models
{
    public class DiscoverMoviesOptions
    {
        public int? GenreId { get; set; }
        public int? Year { get; set; }
        public double? MinRating { get; set; }
        public int? MinVoteCount { get; set; }
        public int? PersonId { get; set; }      // resolved from FindPersonIdAsync
        public int? KeywordId { get; set; }     // resolved from FindKeywordIdAsync
        public string SortBy { get; set; } = "popularity.desc";
        public int Page { get; set; } = 1;
    }
}
