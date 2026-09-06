using Moq;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class SearchMultiQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsResultsFromClient()
        {
            var expected = new List<MultiSearchResult>
            {
                new() { Id = 1, MediaType = "movie", DisplayName = "Fight Club" },
                new() { Id = 2, MediaType = "person", DisplayName = "Brad Pitt" }
            };

            var tmdbMock = new Mock<ITmdbClient>();
            tmdbMock.Setup(c => c.SearchMultiAsync("fight club", 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var handler = new SearchMultiQueryHandler(tmdbMock.Object);
            var result = await handler.Handle(new SearchMultiQuery("fight club"), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.MediaType == "movie");
            Assert.Contains(result, r => r.MediaType == "person");
        }

        [Fact]
        public async Task Handle_PassesPageThrough()
        {
            var tmdbMock = new Mock<ITmdbClient>();
            tmdbMock.Setup(c => c.SearchMultiAsync("dune", 2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MultiSearchResult>());

            var handler = new SearchMultiQueryHandler(tmdbMock.Object);
            await handler.Handle(new SearchMultiQuery("dune", page: 2), CancellationToken.None);

            tmdbMock.Verify(c => c.SearchMultiAsync("dune", 2, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}