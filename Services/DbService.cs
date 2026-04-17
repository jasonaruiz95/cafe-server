using Microsoft.EntityFrameworkCore;

public interface IDbService
{
    Task<IEnumerable<MenuResponse>> GetMenuItemsAsync();
    Task<CartResponse> GetCartAsync();
    Task<CartResponse> CreateCartAsync(string UserId);
    Task<bool> DeleteItemFromCartAsync(int cartItemId);
    Task<bool> UpdateCartItemAsync(UpdateCartItemDTO dto);
    Task<bool> AddItemToCartAsync(UpdateCartItemDTO dto);
    Task<bool> ClearCart(int cartId);

}

public class DbService : IDbService
{
    private readonly ApplicationDbContext _dbContext;

    public DbService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<MenuResponse>> GetMenuItemsAsync()
    {
        return await _dbContext.MenuItems.Select(m => new MenuResponse(m)).ToListAsync();
    }
    public async Task<CartResponse> GetCartAsync()
    {
        return await _dbContext.Carts.Where(c => c.UserId == "5713edb3-ecce-426d-a153-b42e614ac0fb").Include(c => c.CartItems).ThenInclude(c => c.MenuItem).Select(m => new CartResponse(m)).FirstOrDefaultAsync() ?? throw new ArgumentException("No Cart found");
    }
    public async Task<CartResponse> CreateCartAsync(string UserId)
    {

        Cart cart = new Cart()
        {
            UserId = UserId,
        };

        await _dbContext.Carts.AddAsync(cart);
        await _dbContext.SaveChangesAsync();

        return new CartResponse(cart);
    }

    public async Task<bool> AddItemToCartAsync(UpdateCartItemDTO dto)
    {
        int menuItemId, cartId;
        menuItemId = dto.MenuItemId;
        cartId = dto.CartId;

        Cart cart = await _dbContext.Carts.Where(c => c.Id == cartId).FirstOrDefaultAsync() ?? throw new ArgumentException("Cart not found");
        CartItem cartItem = new CartItem()
        {
            CartId = cart.Id,
            MenuItem = await _dbContext.MenuItems.FindAsync(menuItemId) ?? throw new ArgumentException($"Menu Item Id: {menuItemId} not found"),
            Quantity = 1
        };

        // cart.CartItems.Add(cartItem);
        await _dbContext.CartItems.AddAsync(cartItem);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateCartItemAsync(UpdateCartItemDTO dto)
    {
        int menuItemId, cartId;
        menuItemId = dto.MenuItemId;
        cartId = dto.CartId;

        var cartItem = await _dbContext.CartItems.Where(c => c.CartId == cartId).FirstOrDefaultAsync() ?? throw new ArgumentException($"{nameof(UpdateCartItemDTO)} is not a cart item");
        cartItem.Quantity = dto.Quantity;
        cartItem.Notes = dto.Notes;
        // cart.CartItems.Add(cartItem);
        _dbContext.CartItems.Update(cartItem);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteItemFromCartAsync(int cartItemId)
    {
        // Cart cart = await _dbContext.Carts.Where(c => c.Id == cartId).FirstOrDefaultAsync() ?? throw new ArgumentException("Cart not found");
        CartItem cartItem = await _dbContext.CartItems.Where(i => i.Id == cartItemId).FirstOrDefaultAsync() ?? throw new ArgumentException($"Cart item :{cartItemId} not found.");
        _dbContext.CartItems.Remove(cartItem);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public async Task<bool> ClearCart(int cartId)
    {
        var cart = await _dbContext.Carts.Where(c => c.Id == cartId).Include(c => c.CartItems).FirstOrDefaultAsync() ?? throw new ArgumentException($"Cart Id: {cartId} not found");
        cart.CartItems.Clear();
        await _dbContext.SaveChangesAsync();
        return true;
    }


}