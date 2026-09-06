using MovieCatalogue.Application.Queries;

namespace MovieCatalogue.Application.Tests
{
    public class SearchByKeywordQueryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ThrowsOnInvalidKeyword(string? keyword)
        {
            Assert.Throws<ArgumentException>(() => new SearchByKeywordQuery(keyword!));
        }

        [Fact]
        public void Constructor_DefaultsPageToOne()
        {
            var query = new SearchByKeywordQuery("heist");

            Assert.Equal(1, query.Page);
        }
    }
}
