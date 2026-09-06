using MovieCatalogue.Application.Models;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;
using MovieCatalogue.Infrastructure.TMDB.Dtos;

namespace MovieCatalogue.Infrastructure.TMDB
{
    public static class TmdbMovieMapper
    {
        public static Movie ToMovie(this TmdbMovieDetailDto dto)
        {
            var genres = dto.Genres
                .Select(g => new Genre(g.Id, g.Name))
                .ToList();

            var cast = (dto.Credits?.Cast ?? new())
                       .OrderBy(c => c.Order)
                       .Select(c => new CastMember(c.Id, c.Name))
                       .ToList();

            var releaseDate = DateTime.TryParse(dto.ReleaseDate, out var parsed)
                ? parsed
                : default;

            return new Movie(
                id: dto.Id,
                title: dto.Title,
                rating: new Rating(dto.VoteAverage),
                runtime: new Runtime(dto.Runtime),
                releaseDate: releaseDate,
                overview: dto.Overview,
                genres: genres,
                cast: cast);
        }

        public static MovieSummary ToMovieSummary(this TmdbMovieSummaryDto dto)
        {
            return new MovieSummary
            {
                Id = dto.Id,
                Title = dto.Title,
                Overview = dto.Overview,
                PosterUrl = string.IsNullOrEmpty(dto.PosterPath) ? null : dto.PosterPath,
                ReleaseDate = dto.ReleaseDate.HasValue
                    ? DateOnly.FromDateTime(dto.ReleaseDate.Value)
                    : null,
                VoteAverage = dto.VoteAverage,
                VoteCount = dto.VoteCount,
                GenreIds = dto.GenreIds,
                Popularity = dto.Popularity,
            };
        }

    }
}
