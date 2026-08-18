using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class DiscoverMoviesQueryHandler : IRequestHandler<DiscoverMoviesQuery, IReadOnlyList<Movie>>
    {
        private readonly IApplicationDbContext _context;
        public DiscoverMoviesQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Movie>> Handle(DiscoverMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .ToListAsync(cancellationToken);

            var filtered = movies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.Criteria.Title))
                filtered = filtered.Where(m => m.Title.Contains(request.Criteria.Title, StringComparison.OrdinalIgnoreCase));

            if (request.Criteria.GenreId.HasValue)
                filtered = filtered.Where(m => m.Genres.Any(g => g.Id == request.Criteria.GenreId.Value));

            if (request.Criteria.MinRating.HasValue)
                filtered = filtered.Where(m => m.Rating.Value >= request.Criteria.MinRating.Value);

            if (request.Criteria.ActorId.HasValue)
                filtered = filtered.Where(m => m.Cast.Any(c => c.Id == request.Criteria.ActorId.Value));

            return filtered.ToList();
        }
    }
}
