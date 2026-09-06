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

        public static string BuildDiscoverSearchQuery(DiscoverMoviesOptions options)
        {
            var query = new List<string>
            {
                $"page={options.Page}",
                $"sort_by={options.SortBy}"
            };

            if (options.GenreId.HasValue) query.Add($"with_genres={options.GenreId}");
            if (options.Year.HasValue) query.Add($"primary_release_year={options.Year}");
            if (options.MinRating.HasValue) query.Add($"vote_average.gte={options.MinRating}");
            if (options.MinVoteCount.HasValue) query.Add($"vote_count.gte={options.MinVoteCount}");
            if (options.PersonId.HasValue) query.Add($"with_cast={options.PersonId}");
            if (options.KeywordId.HasValue) query.Add($"with_keywords={options.KeywordId}");

            return string.Join("&", query);
        }

        public async Task<IReadOnlyList<MovieSummary>> DiscoverMoviesAsync(
            DiscoverMoviesOptions options, CancellationToken ct = default)
        {
            var url = $"discover/movie?{BuildDiscoverSearchQuery(options)}";

            var response = await _httpClient.GetFromJsonAsync<TmdbPagedResponseDto<TmdbMovieSummaryDto>>(url, ct);

            return response?.Results.Select(dto => dto.ToMovieSummary()).ToList()
                ?? new List<MovieSummary>();
        }

        public async Task<int?> FindPersonIdAsync(string name, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var url = $"search/person?query={Uri.EscapeDataString(name)}";
            var response = await _httpClient.GetFromJsonAsync<TmdbPagedResponseDto<TmdbPersonDto>>(url, ct);

            return response?.Results.FirstOrDefault()?.Id;
        }

        public async Task<int?> FindKeywordIdAsync(string name, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var url = $"search/keyword?query={Uri.EscapeDataString(name)}";
            var response = await _httpClient.GetFromJsonAsync<TmdbPagedResponseDto<TmdbKeywordDto>>(url, ct);

            return response?.Results.FirstOrDefault()?.Id;
        }

        public async Task<IReadOnlyList<MultiSearchResult>> SearchMultiAsync(
            string query, int page = 1, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<MultiSearchResult>();

            var url = $"search/multi?query={Uri.EscapeDataString(query)}&page={page}";
            var response = await _httpClient.GetFromJsonAsync<TmdbPagedResponseDto<TmdbMultiSearchDto>>(url, ct);

            return response?.Results.Select(dto => dto.ToMultiSearchResult()).ToList()
                ?? new List<MultiSearchResult>();
        }
    }
}