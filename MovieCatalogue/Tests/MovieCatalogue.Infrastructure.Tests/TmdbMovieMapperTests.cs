using MovieCatalogue.Infrastructure.TMDB;
using MovieCatalogue.Infrastructure.TMDB.Dtos;

namespace MovieCatalogue.Infrastructure.Tests
{
    public class TmdbMovieMapperTests
    {
        [Fact]
        public void ToMovieSummary_MapsAllScalarFields()
        {
            var dto = new TmdbMovieSummaryDto
            {
                Id = 550,
                Title = "Fight Club",
                Overview = "A depressed man...",
                PosterPath = "/poster.jpg",
                BackdropPath = "/backdrop.jpg",
                ReleaseDate = new DateTime(1999, 10, 15),
                VoteAverage = 8.4,
                VoteCount = 26000,
                Popularity = 61.4,
                GenreIds = new List<int> { 18, 53 }
            };

            var result = dto.ToMovieSummary();

            Assert.Equal(550, result.Id);
            Assert.Equal("Fight Club", result.Title);
            Assert.Equal("A depressed man...", result.Overview);
            Assert.Equal("/poster.jpg", result.PosterUrl);
            Assert.Equal(new DateOnly(1999, 10, 15), result.ReleaseDate);
            Assert.Equal(8.4, result.VoteAverage);
            Assert.Equal(26000, result.VoteCount);
            Assert.Equal(61.4, result.Popularity);
            Assert.Equal(new List<int> { 18, 53 }, result.GenreIds);
        }

        [Fact]
        public void ToMovieSummary_NullReleaseDate_MapsToNull()
        {
            var dto = new TmdbMovieSummaryDto { Id = 1, Title = "Untitled", ReleaseDate = null };

            var result = dto.ToMovieSummary();

            Assert.Null(result.ReleaseDate);
        }

        [Fact]
        public void ToMovieSummary_EmptyPosterPath_MapsToNull()
        {
            var dto = new TmdbMovieSummaryDto { Id = 1, Title = "Untitled", PosterPath = "" };

            var result = dto.ToMovieSummary();

            Assert.Null(result.PosterUrl);
        }

        [Fact]
        public void ToMovieSummary_EmptyGenreIds_MapsToEmptyList()
        {
            var dto = new TmdbMovieSummaryDto { Id = 1, Title = "Untitled", GenreIds = new List<int>() };

            var result = dto.ToMovieSummary();

            Assert.Empty(result.GenreIds);
        }

        [Theory]
        [InlineData("movie")]
        [InlineData("tv")]
        [InlineData("person")]
        public void ToMultiSearchResult_MapsMediaType(string mediaType)
        {
            var dto = new TmdbMultiSearchDto { Id = 1, MediaType = mediaType, Title = "T", Name = "N" };

            var result = dto.ToMultiSearchResult();

            Assert.Equal(mediaType, result.MediaType);
        }

        [Fact]
        public void ToMultiSearchResult_Movie_UsesTitleForDisplayName()
        {
            var dto = new TmdbMultiSearchDto { Id = 1, MediaType = "movie", Title = "Fight Club", Name = null };

            var result = dto.ToMultiSearchResult();

            Assert.Equal("Fight Club", result.DisplayName);
        }

        [Fact]
        public void ToMultiSearchResult_Person_UsesNameForDisplayName()
        {
            var dto = new TmdbMultiSearchDto { Id = 1, MediaType = "person", Title = null, Name = "Brad Pitt" };

            var result = dto.ToMultiSearchResult();

            Assert.Equal("Brad Pitt", result.DisplayName);
        }

        [Fact]
        public void ToMultiSearchResult_Person_UsesProfilePathForImage()
        {
            var dto = new TmdbMultiSearchDto
            {
                Id = 1,
                MediaType = "person",
                Name = "Brad Pitt",
                ProfilePath = "/profile.jpg",
                PosterPath = "/poster.jpg" // should be ignored for person
            };

            var result = dto.ToMultiSearchResult();

            Assert.Equal("/profile.jpg", result.ImageUrl);
        }

        [Fact]
        public void ToMultiSearchResult_Movie_UsesPosterPathForImage()
        {
            var dto = new TmdbMultiSearchDto
            {
                Id = 1,
                MediaType = "movie",
                Title = "Fight Club",
                PosterPath = "/poster.jpg",
                ProfilePath = "/profile.jpg" // should be ignored for movie
            };

            var result = dto.ToMultiSearchResult();

            Assert.Equal("/poster.jpg", result.ImageUrl);
        }
    }
}