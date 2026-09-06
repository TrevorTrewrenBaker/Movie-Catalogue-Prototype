using MovieCatalogue.Application.Queries;

namespace MovieCatalogue.Application.Tests
{
    public class SearchMultiQueryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ThrowsOnInvalidQuery(string? query)
        {
            Assert.Throws<ArgumentException>(() => new SearchMultiQuery(query!));
        }

        [Fact]
        public void Constructor_DefaultsPageToOne()
        {
            var query = new SearchMultiQuery("dune");

            Assert.Equal(1, query.Page);
        }
    }
}