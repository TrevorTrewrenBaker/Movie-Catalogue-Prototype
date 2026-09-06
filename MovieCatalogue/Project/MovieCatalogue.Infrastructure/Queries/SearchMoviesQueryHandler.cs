using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class SearchMoviesQueryHandler : IRequestHandler<SearchMoviesQuery, IReadOnlyList<MovieSummary>>
    {
        private readonly ITmdbClient _tmdbClient;
        private readonly IApplicationDbContext _context;

        public SearchMoviesQueryHandler(ITmdbClient tmdbClient, IApplicationDbContext context)
        {
            _tmdbClient = tmdbClient;
            _context = context;
        }

        public async Task<IReadOnlyList<MovieSummary>> Handle(SearchMoviesQuery request, CancellationToken ct)
        {
            try
            {
                return await _tmdbClient.SearchMoviesAsync(request.Query, request.Page, ct);
            }
            catch (HttpRequestException)
            {
                return await SearchLocalAsync(request, ct);
            }
            catch (TaskCanceledException) when (!ct.IsCancellationRequested)
            {
                return await SearchLocalAsync(request, ct);
            }
        }

        private async Task<IReadOnlyList<MovieSummary>> SearchLocalAsync(SearchMoviesQuery request, CancellationToken ct)
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.Cast)
                .Where(m => m.Title.Contains(request.Query))
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