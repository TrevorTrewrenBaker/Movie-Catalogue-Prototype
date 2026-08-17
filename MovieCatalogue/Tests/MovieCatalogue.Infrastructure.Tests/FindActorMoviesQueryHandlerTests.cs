using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class FindActorMoviesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsEmptyWhenActorNotInAnyMovie()
        {
            using var context = DbContextFactory.Create();
            var movie = new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...",
                genres: [], cast: [new CastMember(500, "Timothée Chalamet")]);
            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            var handler = new FindActorMoviesQueryHandler(context);
            var result = await handler.Handle(new FindActorMoviesQuery("Nonexistent Actor"), CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
