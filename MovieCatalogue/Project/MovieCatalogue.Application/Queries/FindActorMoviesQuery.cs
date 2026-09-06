using MediatR;
using MovieCatalogue.Application.Models;

namespace MovieCatalogue.Application.Queries
{
    public record FindActorMoviesQuery : IRequest<IReadOnlyList<MovieSummary>>
    {
        public string ActorName { get; }
        public int Page { get; }

        public FindActorMoviesQuery(string actorName, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(actorName))
                throw new ArgumentException("Actor name cannot be empty.", nameof(actorName));
            ActorName = actorName;
            Page = page;
        }
    }
}