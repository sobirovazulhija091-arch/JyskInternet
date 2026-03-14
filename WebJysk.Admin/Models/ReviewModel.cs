namespace WebJysk.Admin.Models;

public class ReviewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public UserModel? User { get; set; }
    public Product? Product { get; set; }
}

public class FilterReview
{
    public int? ProductId { get; set; }
    public string? UserId { get; set; }
}
