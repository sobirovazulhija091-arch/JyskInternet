using System.Net.NetworkInformation;

public class CartItem
{
    public int Id { get; set; }
    public int CartId{get;set;}
    public int ProductId { get; set; }
    public int Quantity{get;set;}
    public decimal Price{get;set;}
   public Product? Products { get; set; }
   public Cart? Carts{get;set;}
}

