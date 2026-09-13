using Microsoft.Extensions.Logging;
using MoviesApp.Pages;
using MoviesApp.Services;
using MoviesApp.ViewModels;

namespace MoviesApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(new HttpClient { Timeout = TimeSpan.FromSeconds(15) });
            builder.Services.AddSingleton<IMovieApiService, OmdbApiService>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SearchViewModel>();

            builder.Services.AddTransient<MovieDetailsPage>();
            builder.Services.AddTransient<MovieDetailsViewModel>();

            return builder.Build();
        }
    }
}
