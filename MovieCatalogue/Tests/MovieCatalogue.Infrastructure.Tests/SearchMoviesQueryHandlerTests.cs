using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class SearchMoviesQueryHandlerTests
    {
        //[Fact]
        //public async Task Handle_MatchesPartialTitle()
        //{
        //    using var context = DbContextFactory.Create();
        //    context.Movies.Add(new Movie(1, "Dune: Part Three", new Rating(8.4), new Runtime(155), DateTime.Today, "...", genres: [], cast: []));
        //    await context.SaveChangesAsync();

        //    var handler = new SearchMoviesQueryHandler(context);
        //    var result = await handler.Handle(new SearchMoviesQuery("Part"), CancellationToken.None);

        //    Assert.Single(result);
        //}

        //[Fact]
        //public async Task Handle_ReturnsMultipleMatches()
        //{
        //    using var context = DbContextFactory.Create();
        //    context.Movies.AddRange(
        //        new Movie(1, "Dune: Part One", new Rating(8.0), new Runtime(155), DateTime.Today, "...", genres: [], cast: []),
        //        new Movie(2, "Dune: Part Two", new Rating(8.5), new Runtime(166), DateTime.Today, "...", genres: [], cast: []));
        //    await context.SaveChangesAsync();

        //    var handler = new SearchMoviesQueryHandler(context);
        //    var result = await handler.Handle(new SearchMoviesQuery("Dune"), CancellationToken.None);

        //    Assert.Equal(2, result.Count);
        //}
    }
}
