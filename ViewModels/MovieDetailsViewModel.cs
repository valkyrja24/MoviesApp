using CommunityToolkit.Mvvm.ComponentModel;
using MoviesApp.Models;
using MoviesApp.Services;

namespace MoviesApp.ViewModels
{
    public partial class MovieDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IMovieApiService _movieApiService;

        public MovieDetailsViewModel(IMovieApiService movieApiService)
        {
            _movieApiService = movieApiService;
            Title = "Movie Details";
        }

        [ObservableProperty]
        private MovieDetails? movie;

        [ObservableProperty]
        private bool hasMovie;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private bool hasStatusMessage;

        partial void OnStatusMessageChanged(string value) => HasStatusMessage = !string.IsNullOrWhiteSpace(value);

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("ImdbId", out var value) && value is string imdbId)
            {
                _ = LoadDetailsAsync(imdbId);
            }
        }

        private async Task LoadDetailsAsync(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId) || IsBusy)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;
                HasMovie = false;
                Movie = null;

                var details = await _movieApiService.GetMovieDetailsAsync(imdbId);

                if (details.Response == "True")
                {
                    Movie = details;
                    HasMovie = true;
                    Title = details.Title;
                }
                else
                {
                    StatusMessage = details.Error ?? "Could not load movie details.";
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
    }
}
