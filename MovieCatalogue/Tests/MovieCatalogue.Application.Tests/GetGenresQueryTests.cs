using MovieCatalogue.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class GetGenresQueryTests
    {
        [Fact]
        public void CanBeConstructed()
        {
            var query = new GetGenresQuery();
            Assert.NotNull(query);
        }
    }
}
