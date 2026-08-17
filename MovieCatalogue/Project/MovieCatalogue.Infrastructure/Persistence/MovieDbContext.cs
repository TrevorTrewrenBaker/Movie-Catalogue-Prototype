// MovieCatalogue.Infrastructure/Persistence/MovieDbContext.cs
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.Infrastructure.Persistence
{
    public class MovieDbContext : DbContext, IApplicationDbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<CastMember> CastMembers => Set<CastMember>();
        public DbSet<MoviePreference> MoviePreferences => Set<MoviePreference>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureMovieEntity(modelBuilder);
            ConfigureGenreEntity(modelBuilder);
            ConfigureCastMemberEntity(modelBuilder);
            ConfigureMoviePreferenceEntity(modelBuilder);
        }

        private static void ConfigureMovieEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(m => m.Overview)
                    .HasMaxLength(4000);

                entity.Property(m => m.ReleaseDate)
                    .IsRequired();

                entity.OwnsOne(m => m.Rating, rating =>
                {
                    rating.Property(r => r.Value)
                        .HasColumnName("Rating")
                        .HasPrecision(3, 1)
                        .IsRequired();
                });

                entity.OwnsOne(m => m.Runtime, runtime =>
                {
                    runtime.Property(r => r.Minutes)
                        .HasColumnName("Runtime")
                        .IsRequired();
                });

                entity.HasMany(m => m.Genres)
                      .WithMany()  // No navigation property on Genre side
                      .UsingEntity<Dictionary<string, object>>(
                        "MovieGenre",
                        j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                        j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId")
            );

                entity.HasMany(m => m.Cast)
                    .WithMany()  // No navigation property on CastMember side
                    .UsingEntity<Dictionary<string, object>>(
                        "MovieCast",
                        j => j.HasOne<CastMember>().WithMany().HasForeignKey("CastMemberId"),
                        j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId")
                    );
            });
        }

        private static void ConfigureGenreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(g => g.Name).IsUnique();
            });
        }

        private static void ConfigureCastMemberEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CastMember>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.HasIndex(c => c.Name);
            });
        }

        private static void ConfigureMoviePreferenceEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviePreference>(entity =>
            {
                entity.HasKey(mp => mp.MovieId);

                entity.Property(mp => mp.Preference)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(mp => mp.WatchStatus)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                // FK-only relationship to Movie — no navigation property either side
                entity.HasOne<Movie>()
                    .WithOne()
                    .HasForeignKey<MoviePreference>(mp => mp.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}