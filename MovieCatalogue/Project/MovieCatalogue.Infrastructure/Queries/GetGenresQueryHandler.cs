using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IReadOnlyList<Genre>>
    {
        private readonly IApplicationDbContext _context;
        public GetGenresQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IReadOnlyList<Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            return await _context.Genres.ToListAsync(cancellationToken);
        }
    }
}
