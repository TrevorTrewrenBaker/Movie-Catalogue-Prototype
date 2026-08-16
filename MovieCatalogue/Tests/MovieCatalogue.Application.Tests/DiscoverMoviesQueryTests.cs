using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class DiscoverMoviesQueryTests
    {
        [Fact]
        public void Constructor_SetsCriteria()
        {
            var criteria = new SearchCriteria(genreId: 28);
            var query = new DiscoverMoviesQuery(criteria);
            Assert.Same(criteria, query.Criteria);
        }

        [Fact]
        public void Constructor_ThrowsWhenCriteriaIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DiscoverMoviesQuery(null!));
        }
    }
}
