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
