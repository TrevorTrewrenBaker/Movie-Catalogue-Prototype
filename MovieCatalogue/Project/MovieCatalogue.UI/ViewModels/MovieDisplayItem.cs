using MovieCatalogue.Application.Models;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.UI.ViewModels
{
    public class MovieDisplayItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public double Popularity { get; set; }
        public IReadOnlyList<int> GenreIds { get; set; } = new List<int>();

        public static MovieDisplayItem FromMovie(Movie movie) => new()
        {
            Id = movie.Id,
            Title = movie.Title,
            Overview = movie.Overview,
            PosterUrl = movie.PosterPath,
            ReleaseDate = movie.ReleaseDate,
            VoteAverage = movie.Rating.Value,
            VoteCount = movie.VoteCount,
            Popularity = movie.Popularity,
            GenreIds = movie.Genres.Select(g => g.Id).ToList()
        };

        public static MovieDisplayItem FromSummary(MovieSummary summary) => new()
        {
            Id = summary.Id,
            Title = summary.Title,
            Overview = summary.Overview,
            PosterUrl = summary.PosterUrl,
            ReleaseDate = summary.ReleaseDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
            VoteAverage = summary.VoteAverage,
            VoteCount = summary.VoteCount,
            Popularity = summary.Popularity,
            GenreIds = summary.GenreIds
        };
    }
}