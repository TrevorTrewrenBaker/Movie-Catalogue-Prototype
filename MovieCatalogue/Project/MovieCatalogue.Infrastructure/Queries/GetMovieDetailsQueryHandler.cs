using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class GetMovieDetailsQueryHandler : IRequestHandler<GetMovieDetailsQuery, Movie?>
    {
        private readonly IApplicationDbContext _context;
        public GetMovieDetailsQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<Movie?> Handle(GetMovieDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);
        }
    }
}
