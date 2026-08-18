using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Infrastructure.Persistence
{
    public interface IApplicationDbContext
    {
        // DbSets - Queryable collections for each entity
        DbSet<Movie> Movies { get; }
        DbSet<Genre> Genres { get; }
        DbSet<CastMember> CastMembers { get; }
        DbSet<MoviePreference> MoviePreferences { get; }

        // Save changes
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
