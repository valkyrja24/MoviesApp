using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoviesApp.Models;
using MoviesApp.Pages;
using MoviesApp.Services;

namespace MoviesApp.ViewModels
{
    public partial class SearchViewModel : BaseViewModel
    {
        private readonly IMovieApiService _movieApiService;

        public SearchViewModel(IMovieApiService movieApiService)
        {
            _movieApiService = movieApiService;
            Title = "Movie Search";
        }

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private string statusMessage = "Search for a movie by title to get started.";

        [ObservableProperty]
        private bool hasStatusMessage = true;

        public ObservableCollection<MovieSummary> Movies { get; } = new();

        partial void OnStatusMessageChanged(string value) => HasStatusMessage = !string.IsNullOrWhiteSpace(value);

        [RelayCommand]
        private async Task SearchAsync()
        {
            if (IsBusy)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                StatusMessage = "Enter a movie title to search.";
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;
                Movies.Clear();

                var result = await _movieApiService.SearchMoviesAsync(SearchText);

                if (result.Response == "True" && result.Search.Count > 0)
                {
                    foreach (var movie in result.Search)
                        Movies.Add(movie);
                }
                else
                {
                    StatusMessage = result.Error ?? "No movies found.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Something went wrong: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SelectMovieAsync(MovieSummary? movie)
        {
            if (movie is null || string.IsNullOrWhiteSpace(movie.ImdbId))
                return;

            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}?ImdbId={movie.ImdbId}");
        }
    }
}
