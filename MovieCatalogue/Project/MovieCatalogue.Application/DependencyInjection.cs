using Microsoft.Extensions.DependencyInjection;
using MediatR; 

namespace MovieCatalogue.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Handlers are registered here
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            return services; 
        }
    }
}
