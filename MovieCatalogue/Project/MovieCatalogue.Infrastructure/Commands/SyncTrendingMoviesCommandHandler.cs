using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Application.Commands;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure.Commands
{
    public class SyncTrendingMoviesCommandHandler : IRequestHandler<SyncTrendingMoviesCommand, Unit>
    {
        private readonly ITmdbClient _tmdbClient;
        private readonly IApplicationDbContext _context;

        public SyncTrendingMoviesCommandHandler(ITmdbClient tmdbClient, IApplicationDbContext context)
        {
            _tmdbClient = tmdbClient;
            _context = context;
        }

        public async Task<Unit> Handle(SyncTrendingMoviesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var trendingMovies = await _tmdbClient.GetTrendingAsync(cancellationToken);

                var genreCache = await _context.Genres
                    .ToDictionaryAsync(g => g.Id, g => g, cancellationToken);

                var existingMovies = await _context.Movies
                    .Include(m => m.Genres)
                    .Include(m => m.Cast)
                    .ToDictionaryAsync(m => m.Id, m => m, cancellationToken);

                foreach (var movieSummary in trendingMovies)
                {
                    var releaseDate = movieSummary.ReleaseDate.HasValue
                        ? movieSummary.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue)
                        : DateTime.MinValue;

                    if (existingMovies.TryGetValue(movieSummary.Id, out var existing))
                    {
                        existing.Title = movieSummary.Title;
                        existing.Rating = new Rating(movieSummary.VoteAverage);
                        existing.ReleaseDate = releaseDate;
                        existing.Overview = movieSummary.Overview ?? string.Empty;
                        existing.PosterPath = movieSummary.PosterUrl;
                        existing.VoteCount = movieSummary.VoteCount;
                        existing.Popularity = movieSummary.Popularity;
                        existing.Genres = ResolveGenres(movieSummary.GenreIds, genreCache);

                        continue;
                    }

                    var genres = ResolveGenres(movieSummary.GenreIds, genreCache);

                    var movie = new Movie(
                        movieSummary.Id,
                        movieSummary.Title,
                        new Rating(movieSummary.VoteAverage),
                        new Runtime(0),
                        releaseDate,
                        movieSummary.Overview ?? string.Empty,
                        genres,
                        new List<CastMember>(),
                        movieSummary.PosterUrl,
                        movieSummary.VoteCount,
                        movieSummary.Popularity
                    );

                    _context.Movies.Add(movie);
                    existingMovies[movie.Id] = movie;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to sync trending movies", ex);
            }
        }

        private static List<Genre> ResolveGenres(IReadOnlyList<int> genreIds, Dictionary<int, Genre> genreCache)
        {
            return genreIds
                .Where(id => genreCache.ContainsKey(id))
                .Select(id => genreCache[id])
                .ToList();
        }
    }
}