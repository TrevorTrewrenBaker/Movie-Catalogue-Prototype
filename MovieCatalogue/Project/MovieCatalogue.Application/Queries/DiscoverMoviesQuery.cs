using MediatR;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Application.Queries
{
    public record DiscoverMoviesQuery : IRequest<IReadOnlyList<MovieSummary>>
    {
        public SearchCriteria Criteria { get; }

        public DiscoverMoviesQuery(SearchCriteria criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }
    }
}