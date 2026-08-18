using MediatR;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Application.Queries
{
    public record GetTrendingMoviesQuery : IRequest<IReadOnlyList<Movie>>;
}
