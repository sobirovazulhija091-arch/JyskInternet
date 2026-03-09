public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public decimal DiscountPrice{get;set;}
   public string? ImageUrl { get; set; }    // URL or path to product image
     public int BrandId {get;set;}
     public bool IsActive{get;set;}
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public Brand Brand{get;set;}=null!;
    public List<CartItem> CartItems { get; set; } = [];
    public List<OrderItem> OrderItems { get; set; } = [];
    public List<Review> Reviews{get;set;}=[];
    public List<WarehouseProduct> WarehouseProducts{get;set;}=[];
}



 
