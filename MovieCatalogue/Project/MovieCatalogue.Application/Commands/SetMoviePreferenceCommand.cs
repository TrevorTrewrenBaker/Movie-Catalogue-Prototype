using MediatR;
using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Commands
{
    public record SetMoviePreferenceCommand(int MovieId, PreferenceType Preference) : IRequest;
}
