using MovieCatalogue.Application.Commands;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.Enums;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class SetMoviePreferenceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_SettingSamePreferenceTwiceDoesNotDuplicateRow()
        {
            using var context = DbContextFactory.Create();
            var movie = new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...", genres: [], cast: []);
            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            var handler = new SetMoviePreferenceCommandHandler(context);
            await handler.Handle(new SetMoviePreferenceCommand(movie.Id, PreferenceType.Liked, WatchStatus.Watched), CancellationToken.None);
            await handler.Handle(new SetMoviePreferenceCommand(movie.Id, PreferenceType.Liked, WatchStatus.Watched), CancellationToken.None);

            Assert.Single(context.MoviePreferences);
        }

        [Fact]
        public async Task Handle_PreferenceIsRemovedWhenMovieDeleted()
        {
            using var context = DbContextFactory.Create();
            var movie = new Movie(1, "Dune", new Rating(8.4), new Runtime(155), DateTime.Today, "...", genres: [], cast: []);
            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            var handler = new SetMoviePreferenceCommandHandler(context);
            await handler.Handle(new SetMoviePreferenceCommand(movie.Id, PreferenceType.Liked, WatchStatus.Watched), CancellationToken.None);

            context.Movies.Remove(movie);
            await context.SaveChangesAsync();

            var preference = await context.MoviePreferences.FirstOrDefaultAsync(p => p.MovieId == movie.Id);
            Assert.Null(preference);
        }

    }
}
