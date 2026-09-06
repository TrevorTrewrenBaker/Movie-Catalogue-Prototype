using MovieCatalogue.UI.ViewModels;
using System.Windows;

namespace MovieCatalogue.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Load data when window loads
            Loaded += async (s, e) =>
            {
                await LoadDataAsync().ConfigureAwait(true);
            };
        }

        private async Task LoadDataAsync()
        {
            try
            {
                if (_viewModel.LoadGenresCommand.CanExecute(null))
                {
                    await _viewModel.LoadGenresCommand.ExecuteAsync(null);
                }

                if (_viewModel.LoadTrendingCommand.CanExecute(null))
                {
                    await _viewModel.LoadTrendingCommand.ExecuteAsync(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}