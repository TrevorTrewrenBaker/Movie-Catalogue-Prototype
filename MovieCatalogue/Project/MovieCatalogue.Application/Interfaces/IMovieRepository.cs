using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Application.Interfaces
{
    public interface IMovieRepository
    {
        Task<IReadOnlyList<Movie>> GetTrendingAsync();
        Task<IReadOnlyList<Movie>> SearchAsync(string query);
        Task<Movie?> GetByIdAsync(int id);
        Task<IReadOnlyList<Movie>> DiscoverAsync(SearchCriteria criteria);
        Task<IReadOnlyList<Genre>> GetGenresAsync();
        Task<int?> FindPersonIdAsync(string actorName);
    }
}
