using MediatR;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Application.Queries
{
    public record DiscoverMoviesQuery : IRequest<IReadOnlyList<Movie>>
    {
        public SearchCriteria Criteria { get; }

        public DiscoverMoviesQuery(SearchCriteria criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }
    }
}
