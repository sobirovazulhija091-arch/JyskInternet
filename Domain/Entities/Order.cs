using System.Net;

public class Order
{
    public int Id{get;set;}
    public string? UserId{get;set;}=null!;
    public User User{get;set;}=null!;
    public string DeliveryAddress{get;set;}=null!;

    public EnumPaymentMethod PaymentMethod{get;set;}//enum

    public DateTime OrderDate{get;set;}=DateTime.UtcNow;
    public EnumStatus Status{get;set;}=EnumStatus.Paid;//EnumStatue.Paid; this you can set default value for enum
    public decimal TotalAmount{get;set;}
    public List<OrderItem> OrderItems{get;set;}=[];
    public List<Delivery> Deliveries{get;set;}=[];
    public List<Order> Orders{get;set;}=[];
}


