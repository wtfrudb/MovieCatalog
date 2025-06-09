namespace MovieCatalog.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Quantity { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }

}