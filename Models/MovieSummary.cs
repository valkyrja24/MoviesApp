namespace MoviesApp.Models
{
    public class MovieSummary
    {
        public string Title { get; set; } = string.Empty;

        public string Year { get; set; } = string.Empty;

        public string ImdbId { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Poster { get; set; } = string.Empty;

        public bool HasPoster => !string.IsNullOrWhiteSpace(Poster) && Poster != "N/A";
    }
}
