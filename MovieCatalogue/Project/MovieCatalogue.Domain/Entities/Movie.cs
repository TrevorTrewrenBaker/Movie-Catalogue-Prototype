using MovieCatalogue.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Domain.Entities
{
    public class Movie
    {
        public int Id { get; }
        public string Title { get; }
        public Rating Rating { get; }
        public Runtime Runtime { get; }
        public DateTime ReleaseDate { get; }
        public string Overview { get; }
        public IReadOnlyList<Genre> Genres { get; }
        public IReadOnlyList<CastMember> Cast { get; }   

        public Movie(
            int id, string title, Rating rating, Runtime runtime,
            DateTime releaseDate, string overview,
            IReadOnlyList<Genre> genres, IReadOnlyList<CastMember> cast)
        {
            Id = id; Title = title; Rating = rating; Runtime = runtime;
            ReleaseDate = releaseDate; Overview = overview;
            Genres = genres; Cast = cast;
        }

        public bool IsHighlyRated => Rating.Value >= 8.0;
    }
}
