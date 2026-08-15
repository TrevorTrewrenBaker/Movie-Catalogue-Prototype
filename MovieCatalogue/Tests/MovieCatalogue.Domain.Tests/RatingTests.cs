using MovieCatalogue.Domain.Enums;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Domain.Tests
{
    public class RatingTests
    {
        [Fact]
        public void Constructor_AcceptsValidValue()
        {
            var rating = new Rating(8.5);
            Assert.Equal(8.5, rating.Value);
        }

        [Fact]
        public void Constructor_AcceptsBoundaryValues()
        {
            var min = new Rating(0);
            var max = new Rating(10);

            Assert.Equal(0, min.Value);
            Assert.Equal(10, max.Value);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(-5)]
        public void Constructor_ThrowsWhenBelowZero(double value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rating(value));
        }

        [Theory]
        [InlineData(10.1)]
        [InlineData(15)]
        public void Constructor_ThrowsWhenAboveTen(double value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rating(value));
        }

        [Fact]
        public void ToString_FormatsAsOutOfTen()
        {
            var rating = new Rating(7.2);
            Assert.Equal("7.2/10", rating.ToString());
        }

        [Theory]
        [InlineData(0.0, RatingCategory.Poor)]
        [InlineData(3.9, RatingCategory.Poor)]
        [InlineData(4.0, RatingCategory.Average)]
        [InlineData(5.9, RatingCategory.Average)]
        [InlineData(6.0, RatingCategory.Good)]
        [InlineData(7.9, RatingCategory.Good)]
        [InlineData(8.0, RatingCategory.Excellent)]
        [InlineData(10.0, RatingCategory.Excellent)]
        public void Category_ReturnsCorrectBand(double value, RatingCategory expected)
        {
            var rating = new Rating(value);
            Assert.Equal(expected, rating.Category);
        }
    }
}
