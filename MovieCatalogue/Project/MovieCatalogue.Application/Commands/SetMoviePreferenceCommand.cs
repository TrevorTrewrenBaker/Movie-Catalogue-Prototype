using MediatR;
using MovieCatalogue.Domain.Enums;

namespace MovieCatalogue.Application.Commands
{
    public record SetMoviePreferenceCommand(int MovieId, PreferenceType Preference, WatchStatus WatchStatus) : IRequest;
}
