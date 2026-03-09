namespace WebJysk.Admin.Models;

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

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public decimal DiscountPrice { get; set; }
    public string? ImageUrl { get; set; }
    public int BrandId { get; set; }
    public bool IsActive { get; set; }
}

public class FilterProduct
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? IsActive { get; set; }
}
