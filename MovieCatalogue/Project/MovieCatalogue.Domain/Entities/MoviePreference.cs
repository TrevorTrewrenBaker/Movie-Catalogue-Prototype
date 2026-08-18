using MovieCatalogue.Domain.Enums;

namespace MovieCatalogue.Domain.Entities
{
    public class MoviePreference
    {
        public int MovieId { get; }
        public PreferenceType Preference { get; }
        public WatchStatus WatchStatus { get; }
        public MoviePreference(int movieId, PreferenceType preference, WatchStatus watchStatus)
        {
            MovieId = movieId;
            Preference = preference;
            WatchStatus = watchStatus;
        }
    }
}
