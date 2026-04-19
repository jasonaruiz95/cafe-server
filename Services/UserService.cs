using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    bool IsAuthenticated { get; }
    Task<IdentityUser> GetUserAsync();
    // bool IsAuthenticated();
}

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    private ClaimsPrincipal? Principal =>
        _httpContextAccessor.HttpContext?.User;

    public string? UserId =>
        Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email =>
        Principal?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated =>
        Principal?.Identity?.IsAuthenticated ?? false;

    public async Task<IdentityUser?> GetUserAsync() =>
        await _userManager.GetUserAsync(Principal!);
}