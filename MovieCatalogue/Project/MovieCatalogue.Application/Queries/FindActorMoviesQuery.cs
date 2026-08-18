using MediatR;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Application.Queries
{
    public record FindActorMoviesQuery : IRequest<IReadOnlyList<Movie>>
    {
        public string ActorName { get; }

        public FindActorMoviesQuery(string actorName)
        {
            if (string.IsNullOrWhiteSpace(actorName))
                throw new ArgumentException("Actor name cannot be empty.", nameof(actorName));
            ActorName = actorName;
        }
    }
}
