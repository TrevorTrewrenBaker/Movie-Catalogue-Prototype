using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class GetMoviesByWatchStatusQueryTests
    {
        [Fact]
        public void Constructor_SetsStatus()
        {
            var query = new GetMoviesByWatchStatusQuery(WatchStatus.Watched);

            Assert.Equal(WatchStatus.Watched, query.Status);
        }
    }
}
