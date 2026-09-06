
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using MovieCatalogue.Application.Commands;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Models;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Infrastructure.Commands;
using MovieCatalogue.Infrastructure.Persistence;
using MovieCatalogue.Infrastructure.Queries;
using MovieCatalogue.Infrastructure.Tmdb;
using System.Net.Http.Headers;

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

            // 3. Register TMDB client (interface -> implementation)
            services.AddHttpClient<ITmdbClient, TmdbClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", configuration["Tmdb:AccessToken"]);
            });

            // Register MediatR
            services.AddMediatR(typeof(DependencyInjection).Assembly);

            // MANUALLY register all handlers from Infrastructure
            services.AddScoped<IRequestHandler<GetTrendingMoviesQuery, IReadOnlyList<Movie>>, GetTrendingMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<SearchMoviesQuery, IReadOnlyList<MovieSummary>>, SearchMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<GetMovieDetailsQuery, Movie>, GetMovieDetailsQueryHandler>();
            services.AddScoped<IRequestHandler<GetGenresQuery, IReadOnlyList<Genre>>, GetGenresQueryHandler>();
            services.AddScoped<IRequestHandler<GetMoviesByPreferenceQuery, IReadOnlyList<Movie>>, GetMoviesByPreferenceQueryHandler>();
            services.AddScoped<IRequestHandler<GetMoviesByWatchStatusQuery, IReadOnlyList<Movie>>, GetMoviesByWatchStatusQueryHandler>();
            services.AddScoped<IRequestHandler<DiscoverMoviesQuery, IReadOnlyList<MovieSummary>>, DiscoverMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<FindActorMoviesQuery, IReadOnlyList<MovieSummary>>, FindActorMoviesQueryHandler>();
            services.AddScoped<IRequestHandler<SearchByKeywordQuery, IReadOnlyList<MovieSummary>>, SearchByKeywordQueryHandler>();
            services.AddScoped<IRequestHandler<SearchMultiQuery, IReadOnlyList<MultiSearchResult>>, SearchMultiQueryHandler>();


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