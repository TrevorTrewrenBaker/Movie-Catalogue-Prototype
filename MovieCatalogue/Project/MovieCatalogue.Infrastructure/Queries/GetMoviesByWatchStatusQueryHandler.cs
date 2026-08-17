using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class GetMoviesByWatchStatusQueryHandler : IRequestHandler<GetMoviesByWatchStatusQuery, IReadOnlyList<Movie>>
    {
        private readonly IApplicationDbContext _context;
        public GetMoviesByWatchStatusQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Movie>> Handle(GetMoviesByWatchStatusQuery request, CancellationToken cancellationToken)
        {
            var movieIds = await _context.MoviePreferences
                .Where(p => p.WatchStatus == request.Status)
                .Select(p => p.MovieId)
                .ToListAsync(cancellationToken);

            return await _context.Movies
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
