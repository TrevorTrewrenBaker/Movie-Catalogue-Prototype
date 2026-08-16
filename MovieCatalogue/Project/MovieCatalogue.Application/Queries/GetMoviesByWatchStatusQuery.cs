using MediatR;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.Enums;

namespace MovieCatalogue.Application.Queries
{
    public record GetMoviesByWatchStatusQuery(WatchStatus Status) : IRequest<IReadOnlyList<Movie>>;
}
