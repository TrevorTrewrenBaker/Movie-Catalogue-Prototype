using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbKeywordDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
