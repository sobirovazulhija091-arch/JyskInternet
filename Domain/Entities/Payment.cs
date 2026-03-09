public class Payment
{
  public int Id{get;set;}

  public int OrderId{get;set;}

  public EnumPaymentMethod PaymentMethod{get;set;}//enum

  public EnumPaymentStatus PaymentStatus{get;set;}//enum
  public Order? Order{get;set;}

  public int TransactionId{get;set;}
}