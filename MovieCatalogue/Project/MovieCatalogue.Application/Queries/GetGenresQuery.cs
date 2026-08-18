using MediatR;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Application.Queries
{
    public record GetGenresQuery : IRequest<IReadOnlyList<Genre>>;
}
