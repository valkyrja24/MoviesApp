using MoviesApp.ViewModels;

namespace MoviesApp.Pages
{
    public partial class MovieDetailsPage : ContentPage
    {
        public MovieDetailsPage(MovieDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
