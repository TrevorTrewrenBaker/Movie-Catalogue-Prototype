using MovieCatalogue.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class GetTrendingMoviesQueryTests
    {
        [Fact]
        public void CanBeConstructed()
        {
            var query = new GetTrendingMoviesQuery();
            Assert.NotNull(query);
        }
    }
}
