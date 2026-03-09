public class  UpdateOrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }//use for how many items of the product in the order
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}