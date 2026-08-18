using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Infrastructure.TMDB.Dtos
{
    public class TmdbCreditsDto
    {
        public List<TmdbCastDto> Cast { get; set; } = new();
    }
}
