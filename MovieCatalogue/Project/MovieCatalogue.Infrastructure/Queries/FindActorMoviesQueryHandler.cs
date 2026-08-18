using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class FindActorMoviesQueryHandler : IRequestHandler<FindActorMoviesQuery, IReadOnlyList<Movie>>
    {
        private readonly IApplicationDbContext _context;
        public FindActorMoviesQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Movie>> Handle(FindActorMoviesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Movies
                .Where(m => m.Cast.Any(c => c.Name == request.ActorName))
                .ToListAsync(cancellationToken);
        }
    }
}
