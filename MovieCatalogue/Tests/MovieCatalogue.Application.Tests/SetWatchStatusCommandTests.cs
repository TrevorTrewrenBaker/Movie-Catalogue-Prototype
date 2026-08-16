using MovieCatalogue.Application.Commands;
using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class SetWatchStatusCommandTests
    {
        [Fact]
        public void Constructor_SetsMovieIdAndStatus()
        {
            var command = new SetWatchStatusCommand(42, WatchStatus.Watched);

            Assert.Equal(42, command.MovieId);
            Assert.Equal(WatchStatus.Watched, command.Status);
        }
    }
}
