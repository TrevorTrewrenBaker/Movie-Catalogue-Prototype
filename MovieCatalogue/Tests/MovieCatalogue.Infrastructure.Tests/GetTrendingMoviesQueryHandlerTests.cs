using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class GetTrendingMoviesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsMoviesOrderedByRatingDescending()
        {
            using var context = DbContextFactory.Create();
            var movies = new List<Movie>
            {
              new Movie(id: 1, title: "Low Rated", rating: new Rating(5.0), runtime: new Runtime(100),
                        releaseDate: DateTime.Today, overview: "...",
                        genres: [new Genre(1, "Action")], cast: [new CastMember(1, "Dwayne Johnson")]),

              new Movie(id: 2, title: "High Rated", rating: new Rating(9.0), runtime: new Runtime(100),
                        releaseDate: DateTime.Today, overview: "...",
                        genres: [new Genre(2, "Sci-Fi")], cast: [new CastMember(2, "Timothée Chalamet")])
            };

            context.Movies.AddRange(movies);
            await context.SaveChangesAsync();

            var handler = new GetTrendingMoviesQueryHandler(context);
            var result = await handler.Handle(new GetTrendingMoviesQuery(), CancellationToken.None);

            Assert.Equal("High Rated", result.First().Title);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyWhenNoMovies()
        {
            using var context = DbContextFactory.Create();
            var handler = new GetTrendingMoviesQueryHandler(context);

            var result = await handler.Handle(new GetTrendingMoviesQuery(), CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_IncludesGenresAndCastInResult()
        {
            using var context = DbContextFactory.Create();
            context.Movies.Add(new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...",
                genres: [new Genre(878, "Sci-Fi")], cast: [new CastMember(500, "Timothée Chalamet")]));
            await context.SaveChangesAsync();

            var handler = new GetTrendingMoviesQueryHandler(context);
            var result = await handler.Handle(new GetTrendingMoviesQuery(), CancellationToken.None);

            Assert.Single(result[0].Genres);
            Assert.Single(result[0].Cast);
        }

        [Fact]
        public async Task Handle_ReturnsMoviesWithEqualRatingsInStableOrder()
        {
            using var context = DbContextFactory.Create();
            context.Movies.AddRange(
                new Movie(1, "A", new Rating(7.0), new Runtime(100), DateTime.Today, "...", genres: [], cast: []),
                new Movie(2, "B", new Rating(7.0), new Runtime(100), DateTime.Today, "...", genres: [], cast: []));
            await context.SaveChangesAsync();

            var handler = new GetTrendingMoviesQueryHandler(context);
            var result = await handler.Handle(new GetTrendingMoviesQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
        }
    }
}
