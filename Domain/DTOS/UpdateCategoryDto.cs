
public class UpdateCategoryDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
  
}
