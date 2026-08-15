using Microsoft.Extensions.DependencyInjection;
using MovieCatalogue.UI;

namespace MovieCatalogue.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            //services.AddTransient<MainViewModel>();
            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            return services;
        }
    }
}
