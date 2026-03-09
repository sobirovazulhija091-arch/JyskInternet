using Microsoft.Extensions.Logging.Abstractions;

public class UpdateBrandDto
{
    public int Id{get;set;}

    public string? Name{get;set;}=null!;

   public string? Logo{get;set;}=null!;
}