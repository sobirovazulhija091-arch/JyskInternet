public class Delivery
{
    public int Id{get;set;}

   public int OrderId{get;set;}

  public EnumDeliveryType DeliveryType{get;set;}//enum

  public EnumDeliveryStatus DeliveryStatus{get;set;}//enum

  public DateTime DeliveredAt{get;set;}
  public Order? Order{get;set;}
}