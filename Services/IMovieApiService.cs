using MoviesApp.Models;

namespace MoviesApp.Services
{
    public interface IMovieApiService
    {
        Task<SearchResponse> SearchMoviesAsync(string title, CancellationToken cancellationToken = default);

        Task<MovieDetails> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default);
    }
}
