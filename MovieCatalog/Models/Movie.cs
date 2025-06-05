namespace MovieCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!; 
        public string Topic { get; set; }
        public string MainActors { get; set; }
        public string Director { get; set; }
        public string Scriptwriter { get; set; }
        public string MediaType { get; set; }  // CD / Видеокассета
        public string RecordingCompany { get; set; }
        public int ReleaseYear { get; set; }
        public string ImageUrl { get; set; }
    }

}
