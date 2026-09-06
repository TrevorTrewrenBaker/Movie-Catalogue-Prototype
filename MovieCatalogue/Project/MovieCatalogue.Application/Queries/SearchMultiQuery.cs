using MediatR;
using MovieCatalogue.Application.Models;

namespace MovieCatalogue.Application.Queries
{
    public record SearchMultiQuery : IRequest<IReadOnlyList<MultiSearchResult>>
    {
        public string Query { get; }
        public int Page { get; }

        public SearchMultiQuery(string query, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be empty.", nameof(query));

            Query = query;
            Page = page;
        }
    }
}