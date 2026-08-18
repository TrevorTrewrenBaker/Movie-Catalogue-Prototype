using MovieCatalogue.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class GetMovieDetailsQueryTests
    {
        [Fact]
        public void Constructor_SetsMovieId()
        {
            var query = new GetMovieDetailsQuery(42);

            Assert.Equal(42, query.MovieId);
        }
    }
}
