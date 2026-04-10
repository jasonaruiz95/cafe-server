using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ── MenuItem ─────────────────────────────────────

public class MenuItem
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

// ── CartItem ─────────────────────────────────────

public class CartItem
{
    public int Id { get; set; }

    // Foreign key
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public int Quantity { get; set; } = 1;
    public string? Notes { get; set; }
}

// ── Cart ─────────────────────────────────────────

public class Cart
{
    public int Id { get; set; }

    public string? UserId { get; set; } // null for guest carts

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    // Computed — not stored in DB
    [NotMapped]
    public decimal SubTotal => CartItems.Sum(i => i.MenuItem.Price * i.Quantity);

    [NotMapped]
    public int ItemCount => CartItems.Sum(i => i.Quantity);
}

// ── LineItem ─────────────────────────────────────

public class LineItem
{
    public int Id { get; set; }

    public int? BillId { get; set; }
    public Bill? Bill { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    [Required]
    public string Name { get; set; } = string.Empty; // snapshot of name at time of order

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; } // snapshot of price at time of order

    // Stored as JSON string in DB
    public string SelectedOptionsJson { get; set; } = "{}";

    [NotMapped]
    public Dictionary<string, string> SelectedOptions
    {
        get => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(SelectedOptionsJson) ?? new();
        set => SelectedOptionsJson = System.Text.Json.JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public decimal Total => UnitPrice * Quantity;
}

// ── Bill ─────────────────────────────────────────

public class Bill
{
    public int Id { get; set; }

    public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Tax { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Tip { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }
}

// ── Order ─────────────────────────────────────────

public enum OrderStatus
{
    Pending,
    Confirmed,
    Preparing,
    Ready,
    Completed,
    Cancelled
}

public class Order
{
    public int Id { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public int BillId { get; set; }
    public Bill Bill { get; set; } = null!;

    public string? UserId { get; set; }

    public string? Notes { get; set; }
    public int? TableNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}