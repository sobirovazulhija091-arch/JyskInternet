using System.ComponentModel.DataAnnotations;

namespace WebJysk.Marketplace.Models;

public class CreateOrderDto
{
    public int PaymentMethod { get; set; }
    public int Status { get; set; } = 1;
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Delivery address is required")]
    [MinLength(5, ErrorMessage = "Address must be at least 5 characters")]
    public string DeliveryAddress { get; set; } = null!;

    public List<OrderItemDto> Items { get; set; } = [];
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public OrderUser? User { get; set; }
    public string DeliveryAddress { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public int PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}

public class OrderUser
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Product? Product { get; set; }
}
