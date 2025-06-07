namespace MovieCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Topic { get; set; }
        public string? MainActors { get; set; }
        public string? Director { get; set; }
        public string? Scriptwriter { get; set; } = string.Empty;
        public string? MediaType { get; set; } = string.Empty;
        public string? RecordingCompany { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
    }

}
