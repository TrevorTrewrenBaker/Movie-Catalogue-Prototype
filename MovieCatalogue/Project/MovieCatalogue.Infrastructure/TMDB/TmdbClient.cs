using MovieCatalogue.Infrastructure.TMDB.Dtos;
using System.Net.Http.Json;

namespace MovieCatalogue.Infrastructure.Tmdb
{
    public class TmdbClient
    {
        private readonly HttpClient _httpClient;

        public TmdbClient(HttpClient httpClient)
        {
            _httpClient = httpClient; // BaseAddress + Bearer auth header set at DI registration
        }

        public async Task<TmdbMovieDetailDto> GetMovieDetailAsync(int movieId, CancellationToken ct = default)
        {
            var dto = await _httpClient.GetFromJsonAsync<TmdbMovieDetailDto>(
                $"movie/{movieId}?append_to_response=credits", ct);

            return dto ?? throw new InvalidOperationException(
                $"TMDB returned no data for movie {movieId}");
        }

        public async Task<IReadOnlyList<TmdbMovieSummaryDto>> GetTrendingAsync(CancellationToken ct = default)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbTrendingResponseDto>("trending/movie/week", ct);

            return response?.Results ?? new List<TmdbMovieSummaryDto>();
        }
    }
}