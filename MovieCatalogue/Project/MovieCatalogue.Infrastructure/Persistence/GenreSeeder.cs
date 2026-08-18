using System;// MovieCatalogue.Infrastructure/Persistence/GenreSeeder.cs
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Infrastructure.Persistence
{
    public static class GenreSeeder
    {
        private static readonly (int Id, string Name)[] TmdbMovieGenres =
        {
            (28, "Action"), (12, "Adventure"), (16, "Animation"), (35, "Comedy"),
            (80, "Crime"), (99, "Documentary"), (18, "Drama"), (10751, "Family"),
            (14, "Fantasy"), (36, "History"), (27, "Horror"), (10402, "Music"),
            (9648, "Mystery"), (10749, "Romance"), (878, "Science Fiction"),
            (10770, "TV Movie"), (53, "Thriller"), (10752, "War"), (37, "Western")
        };

        public static async Task SeedAsync(IApplicationDbContext context, CancellationToken ct = default)
        {
            var existingIds = await context.Genres
                .Select(g => g.Id)
                .ToListAsync(ct);

            var missing = TmdbMovieGenres
                .Where(g => !existingIds.Contains(g.Id))
                .Select(g => new Genre(g.Id, g.Name));

            context.Genres.AddRange(missing);

            if (context.Genres.Local.Any() || missing.Any())
                await context.SaveChangesAsync(ct);
        }
    }
}