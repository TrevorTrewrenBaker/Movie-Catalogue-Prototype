using Microsoft.Extensions.DependencyInjection;
using MovieCatalogue.Application;
using MovieCatalogue.Infrastructure;
using System.Windows;

namespace MovieCatalogue.UI
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddApplication();
            services.AddInfrastructure();
            services.AddViewModels();
            services.AddViews();

            Services = services.BuildServiceProvider();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

    }
}