using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class GetMoviesByPreferenceQueryHandler : IRequestHandler<GetMoviesByPreferenceQuery, IReadOnlyList<Movie>>
    {
        private readonly IApplicationDbContext _context;
        public GetMoviesByPreferenceQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Movie>> Handle(GetMoviesByPreferenceQuery request, CancellationToken cancellationToken)
        {
            var movieIds = await _context.MoviePreferences
                .Where(p => p.Preference == request.Preference)
                .Select(p => p.MovieId)
                .ToListAsync(cancellationToken);

            return await _context.Movies
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
