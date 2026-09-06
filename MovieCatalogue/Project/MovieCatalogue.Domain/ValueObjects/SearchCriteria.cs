using MovieCatalogue.Domain.Enums;

namespace MovieCatalogue.Domain.ValueObjects
{
    public sealed class SearchCriteria
    {
        public string? Title { get; }
        public int? GenreId { get; }
        public double? MinRating { get; }
        public int? ActorId { get; }
        public PreferenceType? PreferenceFilter { get; }
        public WatchStatus? WatchStatus { get; }
        public int? Year { get; }
        public string SortBy { get; }
        public int Page { get; }

        public SearchCriteria(string? title = null, int? genreId = null,
            double? minRating = null, int? actorId = null,
            PreferenceType? preferenceFilter = null, WatchStatus? watchStatus = null,
            int? year = null, string sortBy = "popularity.desc", int page = 1)
        {
            Title = title;
            GenreId = genreId;
            MinRating = minRating;
            ActorId = actorId;
            PreferenceFilter = preferenceFilter;
            WatchStatus = watchStatus;
            Year = year;
            SortBy = sortBy;
            Page = page;
        }

        public bool IsEmpty => Title is null && GenreId is null && MinRating is null
            && ActorId is null && PreferenceFilter is null && WatchStatus is null && Year is null;
    }
}