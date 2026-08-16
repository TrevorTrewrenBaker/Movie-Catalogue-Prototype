using MovieCatalogue.Application.Commands;
using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class SetMoviePreferenceCommandTests
    {
        [Fact]
        public void Constructor_SetsMovieIdAndPreference()
        {
            var command = new SetMoviePreferenceCommand(42, PreferenceType.Liked);

            Assert.Equal(42, command.MovieId);
            Assert.Equal(PreferenceType.Liked, command.Preference);
        }
    }
}
