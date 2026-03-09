namespace WebJysk.Marketplace.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public decimal DiscountPrice { get; set; }
    public string? ImageUrl { get; set; }
    public int BrandId { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public Brand? Brand { get; set; }
}
