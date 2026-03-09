public class Cart
{
    public int Id { get; set; }

    public string UserId { get; set; } 
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<CartItem> CartItems{get;set;}=[];
}
