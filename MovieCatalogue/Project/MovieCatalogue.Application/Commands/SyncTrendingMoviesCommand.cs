using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCatalogue.Infrastructure.Commands
{
    public class SyncTrendingMoviesCommand : IRequest<Unit>
    {
    }
}
