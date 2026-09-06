using Moq;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Infrastructure.Queries;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class SearchByKeywordQueryHandlerTests
    {
        [Fact]
        public async Task Handle_KeywordFound_ReturnsDiscoverResults()
        {
            var tmdbMock = new Mock<ITmdbClient>();
            tmdbMock.Setup(c => c.FindKeywordIdAsync("heist", It.IsAny<CancellationToken>()))
                .ReturnsAsync(9715);

            var expected = new List<MovieSummary>
            {
                new() { Id = 1, Title = "Ocean's Eleven" }
            };

            tmdbMock.Setup(c => c.DiscoverMoviesAsync(
                    It.Is<DiscoverMoviesOptions>(o => o.KeywordId == 9715),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var handler = new SearchByKeywordQueryHandler(tmdbMock.Object);
            var result = await handler.Handle(new SearchByKeywordQuery("heist"), CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Ocean's Eleven", result[0].Title);
        }

        [Fact]
        public async Task Handle_KeywordNotFound_ReturnsEmpty()
        {
            var tmdbMock = new Mock<ITmdbClient>();
            tmdbMock.Setup(c => c.FindKeywordIdAsync("zzz-nonexistent", It.IsAny<CancellationToken>()))
                .ReturnsAsync((int?)null);

            var handler = new SearchByKeywordQueryHandler(tmdbMock.Object);
            var result = await handler.Handle(new SearchByKeywordQuery("zzz-nonexistent"), CancellationToken.None);

            Assert.Empty(result);
            tmdbMock.Verify(c => c.DiscoverMoviesAsync(It.IsAny<DiscoverMoviesOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_PassesPageThroughToDiscoverOptions()
        {
            var tmdbMock = new Mock<ITmdbClient>();
            tmdbMock.Setup(c => c.FindKeywordIdAsync("space", It.IsAny<CancellationToken>()))
                .ReturnsAsync(1234);

            tmdbMock.Setup(c => c.DiscoverMoviesAsync(
                    It.Is<DiscoverMoviesOptions>(o => o.Page == 3),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MovieSummary>());

            var handler = new SearchByKeywordQueryHandler(tmdbMock.Object);
            await handler.Handle(new SearchByKeywordQuery("space", page: 3), CancellationToken.None);

            tmdbMock.Verify(c => c.DiscoverMoviesAsync(
                It.Is<DiscoverMoviesOptions>(o => o.Page == 3),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}