using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        private Genre() { }

        public Genre(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name cannot be empty.", nameof(name));

            Id = id;
            Name = name;
        }
    }
}
