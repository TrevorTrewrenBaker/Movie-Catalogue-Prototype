using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbCastDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
