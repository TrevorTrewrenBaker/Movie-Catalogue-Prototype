using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Interfaces
{
    public interface IPreferencesRepository
    {
        Task SetPreferenceAsync(int movieId, PreferenceType preference);
        Task SetWatchStatusAsync(int movieId, WatchStatus status);

        Task<PreferenceType> GetPreferenceAsync(int movieId);
        Task<WatchStatus> GetWatchStatusAsync(int movieId);

        Task<IReadOnlyList<Movie>> GetByPreferenceAsync(PreferenceType preference);
        Task<IReadOnlyList<Movie>> GetByWatchStatusAsync(WatchStatus status);
    }
}
