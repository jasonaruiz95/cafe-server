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

    [AllowAnonymous]
    [HttpPost("items")]
    public async Task<IActionResult> AddCartItem([FromBody] UpdateCartItemDTO dto)
    {
        return new JsonResult(await _dbService.AddItemToCartAsync(dto));
    }
    [AllowAnonymous]
    [HttpPatch("items/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDTO dto)
    {
        return new JsonResult(await _dbService.UpdateCartItemAsync(dto));
    }


    [AllowAnonymous]
    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        return new JsonResult(await _dbService.DeleteItemFromCartAsync(cartItemId));
    }

    [AllowAnonymous]
    [HttpDelete("{cartId}")]
    public async Task<IActionResult> ClearCart(int cartId)
    {
        return new JsonResult(await _dbService.ClearCart(cartId));
    }


}