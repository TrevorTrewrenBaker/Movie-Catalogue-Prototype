using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class DiscoverMoviesQueryHandler : IRequestHandler<DiscoverMoviesQuery, IReadOnlyList<MovieSummary>>
    {
        private readonly ITmdbClient _tmdbClient;
        private readonly IApplicationDbContext _context;

        public DiscoverMoviesQueryHandler(ITmdbClient tmdbClient, IApplicationDbContext context)
        {
            _tmdbClient = tmdbClient;
            _context = context;
        }

        public async Task<IReadOnlyList<MovieSummary>> Handle(DiscoverMoviesQuery request, CancellationToken ct)
        {
            try
            {
                var options = new DiscoverMoviesOptions
                {
                    GenreId = request.Criteria.GenreId,
                    Year = request.Criteria.Year,
                    MinRating = request.Criteria.MinRating,
                    Page = request.Criteria.Page,
                    SortBy = request.Criteria.SortBy
                };

                if (request.Criteria.ActorId.HasValue)
                    options.PersonId = request.Criteria.ActorId;

                return await _tmdbClient.DiscoverMoviesAsync(options, ct);
            }
            catch (HttpRequestException)
            {
                return await QueryLocalAsync(request, ct);
            }
            catch (TaskCanceledException) when (!ct.IsCancellationRequested)
            {
                // timeout, not an actual cancellation request
                return await QueryLocalAsync(request, ct);
            }
        }

        private async Task<IReadOnlyList<MovieSummary>> QueryLocalAsync(DiscoverMoviesQuery request, CancellationToken ct)
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .ToListAsync(ct);

            var filtered = movies.AsEnumerable();

            if (request.Criteria.GenreId.HasValue)
                filtered = filtered.Where(m => m.Genres.Any(g => g.Id == request.Criteria.GenreId.Value));

            if (request.Criteria.MinRating.HasValue)
                filtered = filtered.Where(m => m.Rating.Value >= request.Criteria.MinRating.Value);

            if (request.Criteria.ActorId.HasValue)
                filtered = filtered.Where(m => m.Cast.Any(c => c.Id == request.Criteria.ActorId.Value));

            if (request.Criteria.Year.HasValue)
                filtered = filtered.Where(m => m.ReleaseDate.Year == request.Criteria.Year.Value);

            filtered = request.Criteria.SortBy switch
            {
                "vote_average.desc" => filtered.OrderByDescending(m => m.Rating.Value),
                "release_date.desc" => filtered.OrderByDescending(m => m.ReleaseDate),
                _ => filtered.OrderByDescending(m => m.Popularity)
            };

            return filtered.Select(m => new MovieSummary
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