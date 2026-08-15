using Microsoft.Extensions.DependencyInjection;

namespace MovieCatalogue.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services; 
        }
    }
}
