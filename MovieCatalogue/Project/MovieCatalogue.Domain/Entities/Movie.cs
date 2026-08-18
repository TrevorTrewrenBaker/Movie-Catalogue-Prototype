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
        public string? PosterPath { get; set; }
        public int VoteCount { get; set; }
        public double Popularity { get; set; }

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
            List<CastMember> cast,
            string? posterPath = null,
            int voteCount = 0,
            double popularity = 0)
        {
            Id = id;
            Title = title;
            Rating = rating;
            Runtime = runtime;
            ReleaseDate = releaseDate;
            Overview = overview;
            Genres = genres ?? new();
            Cast = cast ?? new();
            PosterPath = posterPath;
            VoteCount = voteCount;
            Popularity = popularity;
        }

        public bool IsHighlyRated => Rating.Value >= 8.0;
        public string GetDisplayTitle() => $"{Title} ({ReleaseDate.Year})";
    }
}