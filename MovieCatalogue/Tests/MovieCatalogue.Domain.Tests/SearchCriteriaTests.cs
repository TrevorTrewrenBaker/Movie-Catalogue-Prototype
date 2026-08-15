using MovieCatalogue.Domain.Enums;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Domain.Tests
{
    public class SearchCriteriaTests
    {
        [Fact]
        public void IsEmpty_TrueWhenAllFieldsNull()
        {
            var criteria = new SearchCriteria();
            Assert.True(criteria.IsEmpty);
        }

        [Fact]
        public void IsEmpty_TrueWhenAllFieldsNullExplicitly()
        {
            var criteria = new SearchCriteria(null, null, null, null, null, null);
            Assert.True(criteria.IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseWhenTitleSet()
        {
            Assert.False(new SearchCriteria(title: "Dune").IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseWhenGenreSet()
        {
            Assert.False(new SearchCriteria(genreId: 28).IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseWhenMinRatingSet()
        {
            Assert.False(new SearchCriteria(minRating: 7.0).IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseWhenActorIdSet()
        {
            Assert.False(new SearchCriteria(actorId: 500).IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseWhenPreferenceFilterSet()
        {
            Assert.False(new SearchCriteria(preferenceFilter: PreferenceType.Liked).IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseWhenWatchStatusFilterSet()
        {
            Assert.False(new SearchCriteria(watchStatus: WatchStatus.NotWatched).IsEmpty);
        }

        [Fact]
        public void Constructor_AllowsCombiningMultipleCriteria()
        {
            var criteria = new SearchCriteria(
                title: "Dune",
                genreId: 878,
                minRating: 7.0,
                actorId: 500,
                preferenceFilter: PreferenceType.Liked,
                watchStatus: WatchStatus.Watched);

            Assert.Equal("Dune", criteria.Title);
            Assert.Equal(878, criteria.GenreId);
            Assert.Equal(7.0, criteria.MinRating);
            Assert.Equal(500, criteria.ActorId);
            Assert.Equal(PreferenceType.Liked, criteria.PreferenceFilter);
            Assert.Equal(WatchStatus.Watched, criteria.WatchStatus);
            Assert.False(criteria.IsEmpty);
        }

        [Theory]
        [InlineData("Inception", null, null, null, null, null)]
        [InlineData(null, 28, null, null, null, null)]
        [InlineData(null, null, 8.5, null, null, null)]
        [InlineData(null, null, null, 500, null, null)]
        [InlineData(null, null, null, null, PreferenceType.StronglyLiked, null)]
        [InlineData(null, null, null, null, null, WatchStatus.WantToWatch)]
        public void IsEmpty_FalseWhenAnyFieldSet(string? title, int? genreId, double? minRating,
            int? actorId, PreferenceType? preferenceFilter, WatchStatus? watchStatus)
        {
            var criteria = new SearchCriteria(title, genreId, minRating, actorId, preferenceFilter, watchStatus);
            Assert.False(criteria.IsEmpty);
        }
    }
}