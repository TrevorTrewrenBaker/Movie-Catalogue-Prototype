using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.Enums;

namespace MovieCatalogue.Domain.Tests
{
    public class MoviePreferenceTests
    {
        [Fact]
        public void Constructor_SetsMovieIdAndPreference()
        {
            var preference = new MoviePreference(42, PreferenceType.Liked, WatchStatus.Watched);

            Assert.Equal(42, preference.MovieId);
            Assert.Equal(PreferenceType.Liked, preference.Preference);
            Assert.Equal(WatchStatus.Watched, preference.WatchStatus);
        }

        [Theory]
        [InlineData(PreferenceType.None, WatchStatus.WantToWatch)]
        [InlineData(PreferenceType.StronglyDisliked, WatchStatus.Watched)]
        [InlineData(PreferenceType.Disliked, WatchStatus.Watched)]
        [InlineData(PreferenceType.Neutral, WatchStatus.Watched)]
        [InlineData(PreferenceType.Liked, WatchStatus.Watched)]
        [InlineData(PreferenceType.StronglyLiked, WatchStatus.Watched)]
        public void Constructor_AcceptsAllPreferenceTypes(PreferenceType type, WatchStatus watchStatus)
        {
            var preference = new MoviePreference(1, type, watchStatus);

            Assert.Equal(type, preference.Preference);
            Assert.Equal(watchStatus, preference.WatchStatus);
        }
    }
}
