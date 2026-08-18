using MovieCatalogue.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Domain.Tests
{
    public class GenreTests
    {
        [Fact]
        public void Constructor_SetsIdAndName()
        {
            var genre = new Genre(28, "Action");

            Assert.Equal(28, genre.Id);
            Assert.Equal("Action", genre.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ThrowsWhenNameIsEmpty(string? name)
        {
            Assert.Throws<ArgumentException>(() => new Genre(1, name!));
        }
    }
}
