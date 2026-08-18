using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class DiscoverMoviesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_FiltersByGenre()
        {
            using var context = DbContextFactory.Create();
            context.Movies.AddRange(
                new Movie(1, "Action Movie", new Rating(7.0), new Runtime(100), DateTime.Today, "...",
                    genres: [new Genre(28, "Action")], cast: []),
                new Movie(2, "Drama Movie", new Rating(7.0), new Runtime(100), DateTime.Today, "...",
                    genres: [new Genre(18, "Drama")], cast: []));
            await context.SaveChangesAsync();

            var handler = new DiscoverMoviesQueryHandler(context);
            var result = await handler.Handle(new DiscoverMoviesQuery(new SearchCriteria(genreId: 28)), CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Action Movie", result[0].Title);
        }

        [Fact]
        public async Task Handle_FiltersByActor()
        {
            using var context = DbContextFactory.Create();
            context.Movies.AddRange(
                new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...",
                    genres: [], cast: [new CastMember(500, "Timothée Chalamet")]),
                new Movie(2, "Other Film", new Rating(6.0), new Runtime(100), DateTime.Today, "...",
                    genres: [], cast: [new CastMember(600, "Someone Else")]));
            await context.SaveChangesAsync();

            var handler = new DiscoverMoviesQueryHandler(context);
            var result = await handler.Handle(new DiscoverMoviesQuery(new SearchCriteria(actorId: 500)), CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Dune", result[0].Title);
        }

        [Fact]
        public async Task Handle_CombinesMultipleFilters()
        {
            using var context = DbContextFactory.Create();

            var sciFi = new Genre(878, "Sci-Fi");
            var drama = new Genre(18, "Drama");

            context.Movies.AddRange(
                new Movie(1, "Matches Both", new Rating(9.0), new Runtime(100), DateTime.Today, "...", genres: [sciFi], cast: []),
                new Movie(2, "Wrong Genre", new Rating(9.0), new Runtime(100), DateTime.Today, "...", genres: [drama], cast: []),
                new Movie(3, "Low Rating", new Rating(3.0), new Runtime(100), DateTime.Today, "...", genres: [sciFi], cast: []));

            await context.SaveChangesAsync();

            var handler = new DiscoverMoviesQueryHandler(context);
            var criteria = new SearchCriteria(genreId: 878, minRating: 8.0);
            var result = await handler.Handle(new DiscoverMoviesQuery(criteria), CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Matches Both", result[0].Title);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyWhenNoMoviesMatchAnyFilter()
        {
            using var context = DbContextFactory.Create();
            context.Movies.Add(new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...",
                genres: [new Genre(878, "Sci-Fi")], cast: []));
            await context.SaveChangesAsync();

            var handler = new DiscoverMoviesQueryHandler(context);
            var result = await handler.Handle(new DiscoverMoviesQuery(new SearchCriteria(genreId: 99999)), CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
