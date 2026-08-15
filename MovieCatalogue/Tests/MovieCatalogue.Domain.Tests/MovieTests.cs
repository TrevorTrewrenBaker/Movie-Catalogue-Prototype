using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Domain.Tests
{
    public class MovieTests
    {
        private static Movie CreateMovie(
            double rating = 7.0,
            IReadOnlyList<Genre>? genres = null,
            IReadOnlyList<CastMember>? cast = null) => new(
                id: 1,
                title: "Dune: Part Three",
                rating: new Rating(rating),
                runtime: new Runtime(150),
                releaseDate: new DateTime(2026, 3, 1),
                overview: "The saga concludes.",
                genres: genres ?? new List<Genre>(),
                cast: cast ?? new List<CastMember>());

        [Fact]
        public void Constructor_SetsAllProperties()
        {
            var genres = new List<Genre> { new(878, "Science Fiction") };
            var cast = new List<CastMember> { new(500, "Timothée Chalamet") };

            var movie = new Movie(
                id: 42,
                title: "Dune: Part Three",
                rating: new Rating(8.4),
                runtime: new Runtime(155),
                releaseDate: new DateTime(2026, 3, 1),
                overview: "The saga concludes.",
                genres: genres,
                cast: cast);

            Assert.Equal(42, movie.Id);
            Assert.Equal("Dune: Part Three", movie.Title);
            Assert.Equal(8.4, movie.Rating.Value);
            Assert.Equal(155, movie.Runtime.Minutes);
            Assert.Equal(new DateTime(2026, 3, 1), movie.ReleaseDate);
            Assert.Equal("The saga concludes.", movie.Overview);
            Assert.Same(genres, movie.Genres);
            Assert.Same(cast, movie.Cast);
        }

        [Fact]
        public void Constructor_AcceptsEmptyGenresAndCast()
        {
            var movie = CreateMovie(genres: new List<Genre>(), cast: new List<CastMember>());

            Assert.Empty(movie.Genres);
            Assert.Empty(movie.Cast);
        }

        [Theory]
        [InlineData(8.0, true)]
        [InlineData(9.5, true)]
        [InlineData(10.0, true)]
        [InlineData(7.9, false)]
        [InlineData(0.0, false)]
        public void IsHighlyRated_ReflectsRatingThreshold(double ratingValue, bool expected)
        {
            var movie = CreateMovie(rating: ratingValue);

            Assert.Equal(expected, movie.IsHighlyRated);
        }

        [Fact]
        public void Genres_ExposesSameInstancePassedIn()
        {
            var genres = new List<Genre> { new(28, "Action"), new(12, "Adventure") };

            var movie = CreateMovie(genres: genres);

            Assert.Equal(2, movie.Genres.Count);
            Assert.Contains(movie.Genres, g => g.Name == "Action");
            Assert.Contains(movie.Genres, g => g.Name == "Adventure");
        }

        [Fact]
        public void Cast_ExposesSameInstancePassedIn()
        {
            var cast = new List<CastMember> { new(500, "Timothée Chalamet"), new(501, "Zendaya") };

            var movie = CreateMovie(cast: cast);

            Assert.Equal(2, movie.Cast.Count);
            Assert.Contains(movie.Cast, c => c.Name == "Timothée Chalamet");
        }
    }
}
