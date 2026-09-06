using MovieCatalogue.Application.Models;
using MovieCatalogue.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Interfaces
{
    public interface ITmdbClient
    {
        Task<Movie> GetMovieDetailAsync(int movieId, CancellationToken ct = default);
        Task<IReadOnlyList<MovieSummary>> SearchMoviesAsync(string query, int page = 1, CancellationToken ct = default);
        Task<IReadOnlyList<MovieSummary>> GetTrendingAsync(CancellationToken ct = default);
    }
}
