public class DiscountDto
{
     public string Code{get;set;}=null!;

   public int Percentage{get;set;}
    public bool IsActive{get;set;}

   public DateTime ExpiryDate{get;set;}//Срок действия до

}