using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.Enums;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class GetMoviesByPreferenceQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsEmptyWhenNoMoviesMatchPreference()
        {
            using var context = DbContextFactory.Create();
            var movie = new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...", genres: [], cast: []);
            context.Movies.Add(movie);
            context.MoviePreferences.Add(new MoviePreference(movie.Id, PreferenceType.Disliked, WatchStatus.Watched));
            await context.SaveChangesAsync();

            var handler = new GetMoviesByPreferenceQueryHandler(context);
            var result = await handler.Handle(new GetMoviesByPreferenceQuery(PreferenceType.Liked), CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
