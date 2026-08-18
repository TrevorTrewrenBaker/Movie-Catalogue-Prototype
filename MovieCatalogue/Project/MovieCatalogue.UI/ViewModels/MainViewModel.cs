using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;

namespace MovieCatalogue.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IMediator _mediator;
    private List<Movie> _allMovies = new();

    [ObservableProperty]
    private ObservableCollection<Movie> movies = new();

    [ObservableProperty]
    private ObservableCollection<Genre> availableGenres = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private Genre? selectedGenre;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    public MainViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task LoadGenresAsync()
    {
        try
        {
            var genres = await _mediator.Send(new GetGenresQuery());
            AvailableGenres = new ObservableCollection<Genre>(genres.OrderBy(g => g.Name));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }

    [RelayCommand]
    private async Task LoadTrendingAsync()
    {
        await ExecuteAsync(() => _mediator.Send(new GetTrendingMoviesQuery())).ConfigureAwait(true);
    }

    [RelayCommand]
    private void Search()
    {
        ApplyFilter();
    }

    partial void OnSearchQueryChanged(string value) => ApplyFilter();
    partial void OnSelectedGenreChanged(Genre? value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = _allMovies.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchQuery))
            filtered = filtered.Where(m =>
                m.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));

        if (SelectedGenre is not null)
            filtered = filtered.Where(m =>
                m.Genres.Any(g => g.Id == SelectedGenre.Id));

        Movies = new ObservableCollection<Movie>(filtered);
    }

    private async Task ExecuteAsync(Func<Task<IReadOnlyList<Movie>>> action)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var results = await action();
            _allMovies = results.ToList();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Something went wrong loading movies. Please try again.";
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        finally
        {
            IsLoading = false;
        }
    }
}