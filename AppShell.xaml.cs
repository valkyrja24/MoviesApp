using MoviesApp.Pages;

namespace MoviesApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MovieDetailsPage), typeof(MovieDetailsPage));
        }
    }
}
