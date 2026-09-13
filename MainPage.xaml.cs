using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(SearchViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnMovieSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;

            if (e.CurrentSelection.FirstOrDefault() is MovieSummary movie && BindingContext is SearchViewModel viewModel)
            {
                await viewModel.SelectMovieCommand.ExecuteAsync(movie);
            }
        }
    }
}
