using System.Net;

public class OrderDto
{
    public string? UserId { get; set; }
    public EnumPaymentMethod PaymentMethod { get; set; }
    public EnumStatus status { get; set; } = EnumStatus.Paid;
    public decimal TotalAmount { get; set; }
    public string DeliveryAddress { get; set; } = null!;
    public List<OrderItemDto>? Items { get; set; }
}
