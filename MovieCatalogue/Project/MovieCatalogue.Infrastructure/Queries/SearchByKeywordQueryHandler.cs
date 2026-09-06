using MediatR;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class SearchByKeywordQueryHandler : IRequestHandler<SearchByKeywordQuery, IReadOnlyList<MovieSummary>>
    {
        private readonly ITmdbClient _tmdbClient;

        public SearchByKeywordQueryHandler(ITmdbClient tmdbClient) => _tmdbClient = tmdbClient;

        public async Task<IReadOnlyList<MovieSummary>> Handle(SearchByKeywordQuery request, CancellationToken ct)
        {
            var keywordId = await _tmdbClient.FindKeywordIdAsync(request.Keyword, ct);

            if (keywordId is null)
                return new List<MovieSummary>();

            var options = new DiscoverMoviesOptions
            {
                KeywordId = keywordId,
                Page = request.Page
            };

            return await _tmdbClient.DiscoverMoviesAsync(options, ct);
        }
    }
}