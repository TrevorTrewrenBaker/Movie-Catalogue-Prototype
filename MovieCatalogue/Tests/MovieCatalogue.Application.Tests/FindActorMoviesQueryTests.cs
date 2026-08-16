using MovieCatalogue.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Application.Tests
{
    public class FindActorMoviesQueryTests
    {
        [Fact]
        public void Constructor_SetsActorName()
        {
            var query = new FindActorMoviesQuery("Timothée Chalamet");
            Assert.Equal("Timothée Chalamet", query.ActorName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ThrowsWhenActorNameIsEmpty(string? name)
        {
            Assert.Throws<ArgumentException>(() => new FindActorMoviesQuery(name!));
        }
    }
}
