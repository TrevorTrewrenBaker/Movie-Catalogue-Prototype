using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.TMDB.Dtos;
using MovieCatalogue.Infrastructure.TMDB;
using System.Net.Http.Json;

namespace MovieCatalogue.Infrastructure.Tmdb
{
    public class TmdbClient : ITmdbClient
    {
        private readonly HttpClient _httpClient;

        public TmdbClient(HttpClient httpClient)
        {
            _httpClient = httpClient; // BaseAddress + Bearer auth header set at DI registration
        }

        public async Task<Movie> GetMovieDetailAsync(int movieId, CancellationToken ct = default)
        {
            var dto = await _httpClient.GetFromJsonAsync<TmdbMovieDetailDto>(
                $"movie/{movieId}?append_to_response=credits", ct);

            if (dto is null)
                throw new InvalidOperationException($"TMDB returned no data for movie {movieId}");

            return dto.ToMovie();
        }

        public async Task<IReadOnlyList<MovieSummary>> GetTrendingAsync(CancellationToken ct = default)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbPagedResponseDto<TmdbMovieSummaryDto>>(
                "trending/movie/week", ct);

            return response?.Results.Select(dto => dto.ToMovieSummary()).ToList()
                ?? new List<MovieSummary>();
        }

        public async Task<IReadOnlyList<MovieSummary>> SearchMoviesAsync(
            string query, int page = 1, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<MovieSummary>();

            var url = $"search/movie?query={Uri.EscapeDataString(query)}&page={page}";

            var response = await _httpClient.GetFromJsonAsync<TmdbPagedResponseDto<TmdbMovieSummaryDto>>(url, ct);

            return response?.Results.Select(dto => dto.ToMovieSummary()).ToList()
                ?? new List<MovieSummary>();
        }
    }
}