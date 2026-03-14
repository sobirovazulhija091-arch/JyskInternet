namespace WebJysk.Admin.Models;

public class Order
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public UserModel? User { get; set; }
    public string DeliveryAddress { get; set; } = null!;
    public EnumPaymentMethod PaymentMethod { get; set; }
    public DateTime OrderDate { get; set; }
    public EnumStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public enum EnumStatus
{
    Paid = 1,
    Processing = 2,
    Delivered = 3,
    Cancelled = 4
}

public enum EnumPaymentMethod
{
    Cash = 0,
    Card = 1,
    Online = 2
}

public class FilterOrder
{
    public string? UserId { get; set; }
    public EnumStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public decimal? MinTotalAmount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
}
