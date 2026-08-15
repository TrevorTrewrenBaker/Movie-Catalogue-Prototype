using MovieCatalogue.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Domain.ValueObjects
{
    public sealed class SearchCriteria
    {
        public string? Title { get; }
        public int? GenreId { get; }
        public double? MinRating { get; }
        public int? ActorId { get; }
        public PreferenceType? PreferenceFilter { get; }
        public WatchStatus? WatchStatus { get; }

        public SearchCriteria(string? title = null, int? genreId = null,
            double? minRating = null, int? actorId = null,
            PreferenceType? preferenceFilter = null, WatchStatus? watchStatus = null)
        {
            Title = title; 
            GenreId = genreId; 
            MinRating = minRating;
            ActorId = actorId; 
            PreferenceFilter = preferenceFilter;
            WatchStatus = watchStatus;
        }

        public bool IsEmpty => Title is null && GenreId is null && MinRating is null
            && ActorId is null && PreferenceFilter is null && WatchStatus is null; 
    }
}
