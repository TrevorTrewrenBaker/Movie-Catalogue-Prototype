using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class FindActorMoviesQueryHandler : IRequestHandler<FindActorMoviesQuery, IReadOnlyList<MovieSummary>>
    {
        private readonly ITmdbClient _tmdbClient;
        private readonly IApplicationDbContext _context;

        public FindActorMoviesQueryHandler(ITmdbClient tmdbClient, IApplicationDbContext context)
        {
            _tmdbClient = tmdbClient;
            _context = context;
        }

        public async Task<IReadOnlyList<MovieSummary>> Handle(FindActorMoviesQuery request, CancellationToken ct)
        {
            try
            {
                var personId = await _tmdbClient.FindPersonIdAsync(request.ActorName, ct);

                if (personId is null)
                    return await QueryLocalAsync(request, ct);

                var options = new DiscoverMoviesOptions
                {
                    PersonId = personId,
                    Page = request.Page
                };

                return await _tmdbClient.DiscoverMoviesAsync(options, ct);
            }
            catch (HttpRequestException)
            {
                return await QueryLocalAsync(request, ct);
            }
            catch (TaskCanceledException) when (!ct.IsCancellationRequested)
            {
                return await QueryLocalAsync(request, ct);
            }
        }

        private async Task<IReadOnlyList<MovieSummary>> QueryLocalAsync(FindActorMoviesQuery request, CancellationToken ct)
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .Where(m => m.Cast.Any(c => c.Name == request.ActorName))
                .ToListAsync(ct);

            return movies.Select(m => new MovieSummary
            {
                Id = m.Id,
                Title = m.Title,
                Overview = m.Overview,
                PosterUrl = m.PosterPath,
                ReleaseDate = DateOnly.FromDateTime(m.ReleaseDate),
                VoteAverage = m.Rating.Value,
                VoteCount = m.VoteCount,
                Popularity = m.Popularity,
                GenreIds = m.Genres.Select(g => g.Id).ToList()
            }).ToList();
        }
    }
}