using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MovieCatalogue.Application.Interfaces;
using MovieCatalogue.Application.Queries;
using MovieCatalogue.Domain.Entities;
using MovieCatalogue.Domain.ValueObjects;

namespace MovieCatalogue.UI.ViewModels;

public enum MovieViewMode
{
    Trending,
    SearchResults,
    DiscoverResults
}

public partial class MainViewModel : ObservableObject
{
    private readonly IMediator _mediator;
    private readonly ITmdbClient _tmdbClient;
    private List<MovieDisplayItem> _allMovies = new();

    [ObservableProperty]
    private ObservableCollection<MovieDisplayItem> movies = new();

    [ObservableProperty]
    private ObservableCollection<Genre> availableGenres = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private Genre? selectedGenre;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ViewModeLabel))]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ViewModeLabel))]
    private MovieViewMode currentView = MovieViewMode.Trending;

    // Advanced search fields
    [ObservableProperty]
    private bool isAdvancedSearchOpen;

    [ObservableProperty]
    private int? filterYear;

    [ObservableProperty]
    private double? filterMinRating;

    [ObservableProperty]
    private string filterActorName = string.Empty;

    [ObservableProperty]
    private string sortBy = "popularity.desc";

    // Pagination
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages = 1;

    public string ViewModeLabel => CurrentView switch
    {
        MovieViewMode.Trending => "🔥 Trending",
        MovieViewMode.SearchResults => $"🔍 Results for \"{SearchQuery}\"",
        MovieViewMode.DiscoverResults => "🎛️ Filtered Results",
        _ => string.Empty
    };

    public MainViewModel(IMediator mediator, ITmdbClient tmdbClient)
    {
        _mediator = mediator;
        _tmdbClient = tmdbClient;
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
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var results = await _mediator.Send(new GetTrendingMoviesQuery());
            _allMovies = results.Select(MovieDisplayItem.FromMovie).ToList();
            CurrentView = MovieViewMode.Trending;
            CurrentPage = 1;
            TotalPages = 1;
            ApplyFilter();
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            ErrorMessage = "Unable to load trending movies. Please check your connection and try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            await LoadTrendingAsync();
            return;
        }

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var results = await _mediator.Send(new SearchMoviesQuery(SearchQuery));
            _allMovies = results.Select(MovieDisplayItem.FromSummary).ToList();
            CurrentView = MovieViewMode.SearchResults;
            CurrentPage = 1;
            TotalPages = 1;
            ApplyFilter();
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            ErrorMessage = "Unable to search movies. Please check your connection and try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ApplyAdvancedSearchAsync()
    {
        await RunDiscoverAsync(page: 1);
    }

    [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
    private async Task PreviousPageAsync()
    {
        await RunDiscoverAsync(page: CurrentPage - 1);
    }

    private bool CanGoToPreviousPage() => CurrentPage > 1;

    [RelayCommand]
    private async Task NextPageAsync()
    {
        await RunDiscoverAsync(page: CurrentPage + 1);
    }

    private async Task RunDiscoverAsync(int page)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            int? actorId = null;

            if (!string.IsNullOrWhiteSpace(FilterActorName))
            {
                actorId = await _tmdbClient.FindPersonIdAsync(FilterActorName);
            }

            var criteria = new SearchCriteria(
                genreId: SelectedGenre?.Id,
                minRating: FilterMinRating,
                actorId: actorId,
                year: FilterYear,
                sortBy: SortBy,
                page: page);

            var results = await _mediator.Send(new DiscoverMoviesQuery(criteria));

            _allMovies = results.Select(MovieDisplayItem.FromSummary).ToList();
            CurrentView = MovieViewMode.DiscoverResults;
            CurrentPage = page;
            // TMDB caps most discover queries at 500 pages; without a total count from the API response here,
            // we allow forward paging and let an empty result page signal the end.
            TotalPages = _allMovies.Count > 0 ? Math.Max(TotalPages, page) : TotalPages;
            ApplyFilter();
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            ErrorMessage = "Unable to filter movies. Please check your connection and try again.";
        }
        finally
        {
            IsLoading = false;
            PreviousPageCommand.NotifyCanExecuteChanged();
        }
    }

    partial void OnSelectedGenreChanged(Genre? value) => ApplyFilter();

    private void ApplyFilter()
    {
        // Local narrowing only applies when NOT already filtered server-side via Discover
        if (CurrentView == MovieViewMode.DiscoverResults)
        {
            Movies = new ObservableCollection<MovieDisplayItem>(_allMovies);
            return;
        }

        var filtered = _allMovies.AsEnumerable();

        if (SelectedGenre is not null)
            filtered = filtered.Where(m => m.GenreIds.Contains(SelectedGenre.Id));

        Movies = new ObservableCollection<MovieDisplayItem>(filtered);
    }
}