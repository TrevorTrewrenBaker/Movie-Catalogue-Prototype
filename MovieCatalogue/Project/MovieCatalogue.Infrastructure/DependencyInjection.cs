// MovieCatalogue.Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalogue.Infrastructure.Persistence;

namespace MovieCatalogue.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Register DbContext
            services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(MovieDbContext).Assembly.FullName)));

            // 2. Register IApplicationDbContext (Infrastructure interface)
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<MovieDbContext>());

            // 3. Register Repositories (Application Layer interfaces)
            //     services.AddScoped<IMovieRepository, MovieRepository>();
            //     services.AddScoped<IPreferencesRepository, PreferencesRepository>();

            return services;
        }

        // Extension method for IServiceProvider (not IServiceCollection)
        public static void EnsureDatabaseCreated(this IServiceProvider serviceProvider)
        {
            if (serviceProvider is null) return;

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}