using MovieCatalogue.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class SearchMoviesQueryTests
    {
        [Fact]
        public void Constructor_SetsQuery()
        {
            var query = new SearchMoviesQuery("Dune");

            Assert.Equal("Dune", query.Query);
        }
    }
}
