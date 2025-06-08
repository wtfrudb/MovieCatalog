public class RentRequest
{
    public DateTime RentalDate { get; set; }
    public List<RentItemDto> Items { get; set; } = new();
}

public class RentItemDto
{
    public int MovieId { get; set; }
    public int Quantity { get; set; }
    public DateTime? ReturnDate { get; set; }  // новое поле

}

// Новый DTO для ответа (без циклов):
public class RentalOrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime RentalDate { get; set; }
    public List<RentalItemResponseDto> Items { get; set; } = new();
}

public class RentalItemResponseDto
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public int ReleaseYear { get; set; }
    public string MovieDescription { get; set; }
    public int Quantity { get; set; }
    public DateTime? ReturnDate { get; set; }
}
