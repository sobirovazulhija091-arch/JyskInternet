using System.Net;

public class UpdateOrderDto
{
    public int Id{get;set;}
    public string UserId{get;set;}

    public string status{get;set;}//=EnumStatue.Paid;loke this you can set default value for enum
    public decimal TotalAmount{get;set;}

}