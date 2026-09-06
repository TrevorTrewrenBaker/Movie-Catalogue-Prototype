using MediatR;
using MovieCatalogue.Application.Models;

namespace MovieCatalogue.Application.Queries
{
    public record SearchByKeywordQuery : IRequest<IReadOnlyList<MovieSummary>>
    {
        public string Keyword { get; }
        public int Page { get; }

        public SearchByKeywordQuery(string keyword, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword cannot be empty.", nameof(keyword));

            Keyword = keyword;
            Page = page;
        }
    }
}