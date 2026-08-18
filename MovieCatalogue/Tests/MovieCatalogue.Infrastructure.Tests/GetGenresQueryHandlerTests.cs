using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class GetGenresQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllGenresRegardlessOfMovieAssociation()
        {
            using var context = DbContextFactory.Create();
            context.Genres.AddRange(new Genre(1, "Action"), new Genre(2, "Drama"), new Genre(3, "Comedy"));
            await context.SaveChangesAsync();

            var handler = new GetGenresQueryHandler(context);
            var result = await handler.Handle(new GetGenresQuery(), CancellationToken.None);

            Assert.Equal(3, result.Count);
        }
    }
}
