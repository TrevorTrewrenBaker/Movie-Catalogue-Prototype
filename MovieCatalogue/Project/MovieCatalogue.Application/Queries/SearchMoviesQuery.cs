using MediatR;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Application.Queries
{
    public record SearchMoviesQuery : IRequest<IReadOnlyList<Movie>>
    {
        public string Query { get; }

        public SearchMoviesQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be empty.", nameof(query));
            Query = query;
        }
    }
}
