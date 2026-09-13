namespace MoviesApp.Models
{
    public class SearchResponse
    {
        public List<MovieSummary> Search { get; set; } = new();

        public string TotalResults { get; set; } = "0";

        public string Response { get; set; } = "False";

        public string? Error { get; set; }
    }
}
