using MediatR;
using MovieCatalogue.Application.Commands;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.Enums;
using MovieCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Domain.Exceptions;

namespace MovieCatalogue.Infrastructure.Commands
{
    public class SetMoviePreferenceCommandHandler : IRequestHandler<SetMoviePreferenceCommand>
    {
        private readonly IApplicationDbContext _context;

        public SetMoviePreferenceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetMoviePreferenceCommand request, CancellationToken cancellationToken)
        {
            // Validate input
            if (request.MovieId <= 0)
                throw new ArgumentException("MovieId must be greater than 0", nameof(request.MovieId));

            if (request.Preference == PreferenceType.None)
                throw new ArgumentException("Preference cannot be None", nameof(request.Preference));

            // Find the movie
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);

            if (movie is null)
                throw new NotFoundException($"Movie with ID {request.MovieId} not found");

            // Update or create preference
            var existingPreference = await _context.MoviePreferences
                .FirstOrDefaultAsync(p => p.MovieId == request.MovieId, cancellationToken);

            if (existingPreference is null)
            {
                // Create new preference
                var preference = new MoviePreference(request.MovieId, request.Preference, request.WatchStatus);
                _context.MoviePreferences.Add(preference);
            }
            else
            {
                // Update existing preference
                // Assuming MoviePreference has a method to update or is mutable
                // Option 1: If immutable, replace the entity
                _context.MoviePreferences.Remove(existingPreference);
                var newPreference = new MoviePreference(request.MovieId, request.Preference, request.WatchStatus);
                _context.MoviePreferences.Add(newPreference);

                // Option 2: If mutable, update directly
                // existingPreference.UpdatePreference(request.Preference);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
