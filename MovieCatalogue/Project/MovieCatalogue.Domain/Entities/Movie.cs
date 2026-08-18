// MovieCatalogue.Domain/Entities/Movie.cs
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Rating Rating { get; set; }
        public Runtime Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Overview { get; set; } = string.Empty;

        // ✅ Change from IReadOnlyList to List (or ICollection)
        public List<Genre> Genres { get; set; } = new();
        public List<CastMember> Cast { get; set; } = new();

        private Movie() { }

        public Movie(
            int id,
            string title,
            Rating rating,
            Runtime runtime,
            DateTime releaseDate,
            string overview,
            List<Genre> genres,
            List<CastMember> cast)
        {
            Id = id;
            Title = title;
            Rating = rating;
            Runtime = runtime;
            ReleaseDate = releaseDate;
            Overview = overview;
            Genres = genres ?? new();
            Cast = cast ?? new();
        }

        public bool IsHighlyRated => Rating.Value >= 8.0;
        public string GetDisplayTitle() => $"{Title} ({ReleaseDate.Year})";
    }
}