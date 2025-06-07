using MovieCatalog.Models;

public class RentalItem
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int RentalOrderId { get; set; }
    public int Quantity { get; set; }
    public DateTime? ReturnDate { get; set; } // безопасно

    public Movie Movie { get; set; }
    public RentalOrder RentalOrder { get; set; }
}
