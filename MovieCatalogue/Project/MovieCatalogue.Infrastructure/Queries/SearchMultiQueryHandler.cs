using MediatR;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;

namespace MovieCatalogue.Infrastructure.Queries
{
    public class SearchMultiQueryHandler : IRequestHandler<SearchMultiQuery, IReadOnlyList<MultiSearchResult>>
    {
        private readonly ITmdbClient _tmdbClient;

        public SearchMultiQueryHandler(ITmdbClient tmdbClient)
        {
            _tmdbClient = tmdbClient;
        }

        public async Task<IReadOnlyList<MultiSearchResult>> Handle(SearchMultiQuery request, CancellationToken ct)
        {
            return await _tmdbClient.SearchMultiAsync(request.Query, request.Page, ct);
        }
    }
}