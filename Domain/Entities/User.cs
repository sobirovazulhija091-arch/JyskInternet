using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string FullName { get; set; } = null!;
    public string? Phone {get;set;}
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public List<Cart> Carts { get; set; } = [];
    public List<Order> Orders { get; set; } =[];
    public List<Review> Reviews{get;set;}=[]; 
}
