public class Review//(Отзывы)
{
    public int Id{get;set;}

    public int ProductId{get;set;}

    public string UserId{get;set;}=null!;

    public int Rating{get;set;}

    public string Comment{get;set;}=null!;
    public Product? Product{get;set;}
    public User User{get;set;}=null!;
} 




