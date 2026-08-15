using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.Domain.Tests
{
    public class RuntimeTests
    {
        [Fact]
        public void Constructor_AcceptsValidValue()
        {
            var runtime = new Runtime(142);
            Assert.Equal(142, runtime.Minutes);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Constructor_ThrowsWhenNegative(int minutes)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Runtime(minutes));
        }

        [Theory]
        [InlineData(142, "2h 22m")]
        [InlineData(120, "2h 0m")]
        [InlineData(45, "0h 45m")]
        [InlineData(0, "0h 0m")]
        public void ToDisplayString_FormatsCorrectly(int minutes, string expected)
        {
            var runtime = new Runtime(minutes);
            Assert.Equal(expected, runtime.ToDisplayString());
        }
    }
}
