public class RentRequest
{
    public List<RentItemDto> Items { get; set; } = new();
}

public class RentItemDto
{
    public int MovieId { get; set; }
    public int Quantity { get; set; }
}
