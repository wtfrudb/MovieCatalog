using MovieCatalog.Models;
using System.ComponentModel.DataAnnotations.Schema;

// Models/RentalOrder.cs
public class RentalOrder
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Column("RentalDate")]
    public DateTime RentalDate { get; set; }

    public List<RentalItem> Items { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}