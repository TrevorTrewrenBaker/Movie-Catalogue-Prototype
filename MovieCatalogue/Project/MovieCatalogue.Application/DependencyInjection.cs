using Microsoft.Extensions.DependencyInjection;

namespace MovieCatalogue.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services; 
        }
    }
}
