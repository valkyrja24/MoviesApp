using System.Text.Json;
using MoviesApp.Configuration;
using MoviesApp.Models;

namespace MoviesApp.Services
{
    public class OmdbApiService : IMovieApiService
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public OmdbApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SearchResponse> SearchMoviesAsync(string title, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new SearchResponse { Response = "False", Error = "Enter a movie title to search." };

            if (IsMissingApiKey())
                return new SearchResponse { Response = "False", Error = MissingApiKeyMessage };

            var url = $"{ApiSettings.OmdbBaseUrl}?apikey={ApiSettings.OmdbApiKey}&type=movie&s={Uri.EscapeDataString(title)}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<SearchResponse>(stream, JsonOptions, cancellationToken);

            return result ?? new SearchResponse { Response = "False", Error = "Unexpected empty response from server." };
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
                return new MovieDetails { Response = "False", Error = "No movie selected." };

            if (IsMissingApiKey())
                return new MovieDetails { Response = "False", Error = MissingApiKeyMessage };

            var url = $"{ApiSettings.OmdbBaseUrl}?apikey={ApiSettings.OmdbApiKey}&plot=full&i={Uri.EscapeDataString(imdbId)}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<MovieDetails>(stream, JsonOptions, cancellationToken);

            return result ?? new MovieDetails { Response = "False", Error = "Unexpected empty response from server." };
        }

        private static bool IsMissingApiKey() =>
            string.IsNullOrWhiteSpace(ApiSettings.OmdbApiKey) || ApiSettings.OmdbApiKey == "YOUR_OMDB_API_KEY";

        private const string MissingApiKeyMessage =
            "Add a free OMDb API key in Configuration/ApiSettings.cs (get one at https://www.omdbapi.com/apikey.aspx).";
    }
}
