namespace WebJysk.Admin.Models;

public class Brand
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Logo { get; set; }
}

public class BrandDto
{
    public string? Name { get; set; }
    public string? Logo { get; set; }
}
