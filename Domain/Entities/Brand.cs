using Microsoft.Extensions.Logging.Abstractions;

public class Brand
{
    public int Id{get;set;}

    public string? Name{get;set;}=null!;

   public string? Logo{get;set;}=null!;
   public List<Product> Products{get;set;}=[];
}