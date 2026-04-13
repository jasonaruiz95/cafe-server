using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafe_server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IDbService _dbService;
    public MenuController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
        return new JsonResult(await _dbService.GetMenuItemsAsync());
    }
}