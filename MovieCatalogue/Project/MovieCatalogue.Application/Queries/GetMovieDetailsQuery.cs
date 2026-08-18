using MediatR;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Application.Queries
{
    public record GetMovieDetailsQuery(int MovieId) : IRequest<Movie?>;
}
