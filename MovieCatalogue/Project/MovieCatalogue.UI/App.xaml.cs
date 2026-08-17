using Microsoft.Extensions.Configuration;
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
            base.OnStartup(e);

            // Build configuration for WPF
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // Register layers
            services.AddApplication();
            services.AddInfrastructure(configuration);

            // Register UI components
            services.AddViewModels();
            services.AddViews();

            Services = services.BuildServiceProvider();

            // Ensure database is created
            Services.EnsureDatabaseCreated();

            // Start the application
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            (Services as IDisposable)?.Dispose();
            base.OnExit(e);
        }
    }
}