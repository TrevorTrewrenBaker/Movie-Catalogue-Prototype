using MovieCatalogue.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Domain.Tests
{
    public class CastMemberTests
    {
        [Fact]
        public void Constructor_SetsIdAndName()
        {
            var castMember = new CastMember(500, "Timothée Chalamet");

            Assert.Equal(500, castMember.Id);
            Assert.Equal("Timothée Chalamet", castMember.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ThrowsWhenNameIsEmpty(string? name)
        {
            Assert.Throws<ArgumentException>(() => new CastMember(500, name!));
        }
    }
}
