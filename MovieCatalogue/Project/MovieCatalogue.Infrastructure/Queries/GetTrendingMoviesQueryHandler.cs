using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class GetTrendingMoviesQueryHandler : IRequestHandler<GetTrendingMoviesQuery, IReadOnlyList<Movie>>
    {
        private readonly IApplicationDbContext _context;
        public GetTrendingMoviesQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Movie>> Handle(GetTrendingMoviesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .OrderByDescending(m => m.Rating.Value)
                .ToListAsync(cancellationToken);
        }
    }
}
