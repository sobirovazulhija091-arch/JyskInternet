using System.Net.NetworkInformation;

public class UpdateCartItemDto
{
    public int Id { get; set; }
    public int CardId{get;set;}
    public int ProductId { get; set; }
    public int Quantity{get;set;}
    public decimal Price{get;set;}
}