using MovieCatalogue.Application.Models;
using MovieCatalogue.Infrastructure.Tmdb;
using Xunit;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class TmdbClientDiscoverSearchTests
    {
        [Fact]
        public void DefaultOptions_ProducesPageAndSortOnly()
        {
            var options = new DiscoverMoviesOptions();

            var query = TmdbClient.BuildDiscoverSearchQuery(options);

            Assert.Equal("page=1&sort_by=popularity.desc", query);
        }

        [Fact]
        public void GenreId_AddsWithGenresParam()
        {
            var options = new DiscoverMoviesOptions { GenreId = 28 };

            var query = TmdbClient.BuildDiscoverSearchQuery(options);

            Assert.Contains("with_genres=28", query);
        }

        [Fact]
        public void Year_AddsPrimaryReleaseYearParam()
        {
            var options = new DiscoverMoviesOptions { Year = 1999 };

            Assert.Contains("primary_release_year=1999", TmdbClient.BuildDiscoverSearchQuery(options));
        }

        [Fact]
        public void MinRating_AddsVoteAverageGteParam()
        {
            var options = new DiscoverMoviesOptions { MinRating = 7.5 };

            Assert.Contains("vote_average.gte=7.5", TmdbClient.BuildDiscoverSearchQuery(options));
        }

        [Fact]
        public void MinVoteCount_AddsVoteCountGteParam()
        {
            var options = new DiscoverMoviesOptions { MinVoteCount = 1000 };

            Assert.Contains("vote_count.gte=1000", TmdbClient.BuildDiscoverSearchQuery(options));
        }

        [Fact]
        public void PersonId_AddsWithCastParam()
        {
            var options = new DiscoverMoviesOptions { PersonId = 500 };

            Assert.Contains("with_cast=500", TmdbClient.BuildDiscoverSearchQuery(options));
        }

        [Fact]
        public void KeywordId_AddsWithKeywordsParam()
        {
            var options = new DiscoverMoviesOptions { KeywordId = 9715 };

            Assert.Contains("with_keywords=9715", TmdbClient.BuildDiscoverSearchQuery(options));
        }

        [Fact]
        public void CombinedOptions_AddsAllParamsTogether()
        {
            var options = new DiscoverMoviesOptions
            {
                GenreId = 28,
                Year = 2020,
                MinRating = 6.0,
                PersonId = 100,
                SortBy = "vote_average.desc",
                Page = 2
            };

            var query = TmdbClient.BuildDiscoverSearchQuery(options);

            Assert.Equal(
                "page=2&sort_by=vote_average.desc&with_genres=28&primary_release_year=2020&vote_average.gte=6&with_cast=100",
                query);
        }

        [Fact]
        public void NoOptionalFilters_DoesNotAddUnrelatedParams()
        {
            var options = new DiscoverMoviesOptions { GenreId = 28 };

            var query = TmdbClient.BuildDiscoverSearchQuery(options);

            Assert.DoesNotContain("with_cast", query);
            Assert.DoesNotContain("with_keywords", query);
        }
    }
}