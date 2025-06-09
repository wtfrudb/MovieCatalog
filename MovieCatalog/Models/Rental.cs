namespace MovieCatalog.Models
{
    public class Rental
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

}