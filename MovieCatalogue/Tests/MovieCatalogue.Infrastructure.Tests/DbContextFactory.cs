using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure.Tests
{
    public static class DbContextFactory
    {
        public static MovieDbContext Create()
        {
            var options = new DbContextOptionsBuilder<MovieDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MovieDbContext(options);
        }
    }
}
