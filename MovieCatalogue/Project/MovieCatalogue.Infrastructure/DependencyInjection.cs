
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalogue.Application.Commands;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Commands;
using MovieCatalogue.Infrastructure.Persistence;
using MovieCatalogue.Infrastructure.Queries;

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

            // Register MediatR
            services.AddMediatR(typeof(DependencyInjection).Assembly);

            // MANUALLY register all handlers from Infrastructure
            services.AddScoped<IRequestHandler<GetTrendingMoviesQuery, IReadOnlyList<Movie>>, GetTrendingMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<SearchMoviesQuery, IReadOnlyList<Movie>>, SearchMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<DiscoverMoviesQuery, IReadOnlyList<Movie>>, DiscoverMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<GetMovieDetailsQuery, Movie>, GetMovieDetailsQueryHandler>();
            services.AddScoped<IRequestHandler<GetGenresQuery, IReadOnlyList<Genre>>, GetGenresQueryHandler>();
            services.AddScoped<IRequestHandler<GetMoviesByPreferenceQuery, IReadOnlyList<Movie>>, GetMoviesByPreferenceQueryHandler>();
            services.AddScoped<IRequestHandler<GetMoviesByWatchStatusQuery, IReadOnlyList<Movie>>, GetMoviesByWatchStatusQueryHandler>();
            services.AddScoped<IRequestHandler<FindActorMoviesQuery, IReadOnlyList<Movie>>, FindActorMoviesQueryHandler>();

            // Commands
            services.AddScoped<IRequestHandler<SetMoviePreferenceCommand, Unit>, SetMoviePreferenceCommandHandler>();
            services.AddScoped<IRequestHandler<SyncTrendingMoviesCommand, Unit>, SyncTrendingMoviesCommandHandler>();

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