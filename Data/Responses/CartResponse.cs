public class CartResponse
{
    public CartResponse(Cart cart)
    {
        Id = cart.Id;
        UserId = cart.UserId;
        CartItems = cart.CartItems;

    }
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


}

