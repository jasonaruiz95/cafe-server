public class UpdateCartItemDTO
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }

    public string? Notes { get; set; }
    public int CartId { get; set; }

}