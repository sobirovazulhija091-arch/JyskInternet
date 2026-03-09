using System.Net;

public class OrderDto
{
   public string? UserId{get;set;}
   public  EnumPaymentMethod PaymentMethod{get;set;}
    public EnumStatus status{get;set;}//=EnumStatue.Paid; this you can set default value for enum
    public decimal TotalAmount{get;set;}
     public string DeliveryAddress{get;set;}=null!;
 
}
