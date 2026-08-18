using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalogue.Application;
using MovieCatalogue.Infrastructure;
using MovieCatalogue.Infrastructure.Commands;
using MovieCatalogue.Infrastructure.Persistence;
using MovieCatalogue.Infrastructure.Tmdb;
using System.Net.Http.Headers;
using System.Windows;
namespace MovieCatalogue.UI
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider Services { get; private set; } = null!;

        public static string CurrentTheme { get; set; } = "Light";

        private const string tmdbAccessToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3MTk0NTVmZDgwNWQ1MzZlZDZhNGQ5OTRhM2UzNjI3YSIsIm5iZiI6MTc4NjcwMjg4Ny4wMzQsInN1YiI6IjZhN2VlYzI3YjEwZDBiZDA5OGJmOGQ5NiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.d-BXhmFbeWc0C-5B5GAbtlOJ7Dwi2rWpbpMo21uWYKc"; 

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Apply saved theme on startup
            ApplyTheme(CurrentTheme);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddApplication();
            services.AddInfrastructure(configuration);

            services.AddViewModels();
            services.AddViews();

            services.AddHttpClient<TmdbClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tmdbAccessToken);
            });


            Services = services.BuildServiceProvider();

            Services.EnsureDatabaseCreated();

            GenreSeedGenerator(); 

            // Fire-and-forget: UI shows cached data immediately via GetTrendingMoviesQuery,
            // this quietly refreshes the local DB in the background.
            var mediator = Services.GetRequiredService<IMediator>();
            _ = mediator.Send(new SyncTrendingMoviesCommand());

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        public async void GenreSeedGenerator()
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                await GenreSeeder.SeedAsync(context);
            }
        }

        public static void SwitchToDarkTheme()
        {
            CurrentTheme = "Dark";
            ApplyTheme("Dark");
        }

        public static void SwitchToLightTheme()
        {
            CurrentTheme = "Light";
            ApplyTheme("Light");
        }

        public static void ToggleTheme()
        {
            if (CurrentTheme == "Light")
                SwitchToDarkTheme();
            else
                SwitchToLightTheme();
        }

        private static void ApplyTheme(string theme)
        {
            var app = Current as App;
            if (app is null) return;

            // Apply WPF-UI theme
            var wpfUiTheme = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? Wpf.Ui.Appearance.ApplicationTheme.Dark
                : Wpf.Ui.Appearance.ApplicationTheme.Light;

            Wpf.Ui.Appearance.ApplicationThemeManager.Apply(wpfUiTheme);

            var mergedDictionaries = app.Resources.MergedDictionaries;

            // Remove old theme dictionaries
            var toRemove = mergedDictionaries
                .Where(d => d.Source?.OriginalString?.Contains("/Styles/Theme.") == true ||
                            d.Source?.OriginalString?.Contains("/Styles/ElevationEffects.") == true)
                .ToList();

            foreach (var dict in toRemove)
            {
                mergedDictionaries.Remove(dict);
            }

            // Load new theme dictionaries
            try
            {
                // Load Theme.*.xaml
                mergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri($"pack://application:,,,/MovieCatalogue.UI;component/Styles/Theme.{theme}.xaml")
                });

                // Load ElevationEffects.*.xaml
                mergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri($"pack://application:,,,/MovieCatalogue.UI;component/Styles/ElevationEffects.{theme}.xaml")
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load theme resources: {ex.Message}");
                // Optionally show a message box for debugging
                // MessageBox.Show($"Failed to load {theme} theme: {ex.Message}", "Theme Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            (Services as IDisposable)?.Dispose();
            base.OnExit(e);
        }
    }
}