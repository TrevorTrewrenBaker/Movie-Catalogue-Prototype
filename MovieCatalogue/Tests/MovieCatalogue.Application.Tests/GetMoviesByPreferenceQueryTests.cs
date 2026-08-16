using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class GetMoviesByPreferenceQueryTests
    {
        [Fact]
        public void Constructor_SetsPreference()
        {
            var query = new GetMoviesByPreferenceQuery(PreferenceType.Liked);

            Assert.Equal(PreferenceType.Liked, query.Preference);
        }
    }
}
