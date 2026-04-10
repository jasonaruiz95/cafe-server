using Microsoft.EntityFrameworkCore;

public interface IDbService
{
    Task<IEnumerable<MenuResponse>> GetMenuItemsAsync();
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
}