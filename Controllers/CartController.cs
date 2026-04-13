using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafe_server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IDbService _dbService;
    public CartController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        return new JsonResult(await _dbService.GetCartAsync());
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddCartItem(int menuItemId, int cartId)
    {
        return new JsonResult(await _dbService.AddItemToCartAsync(menuItemId, cartId));
    }


    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> DeleteCartItem(int cartId, int cartItemId)
    {
        return new JsonResult(await _dbService.DeleteItemFromCartAsync(cartId, cartItemId));
    }


}