public class MenuResponse
{
    public MenuResponse(MenuItem menuItem)
    {
        Id = menuItem.Id;
        Name = menuItem.Name;
        Description = menuItem.Description;
        Price = menuItem.Price;
        Category = menuItem.Category;
        Image = menuItem.Image;
    }
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

}