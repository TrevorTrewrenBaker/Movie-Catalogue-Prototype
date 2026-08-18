using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class SearchMoviesQueryHandler : IRequestHandler<SearchMoviesQuery, IReadOnlyList<Movie>>
    {
        private readonly IApplicationDbContext _context;
        public SearchMoviesQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Movie>> Handle(SearchMoviesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .Where(m => m.Title.Contains(request.Query))
                .ToListAsync(cancellationToken);
        }
    }
}
