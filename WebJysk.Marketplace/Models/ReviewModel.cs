namespace WebJysk.Marketplace.Models;

public class ReviewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public ReviewUser? User { get; set; }
}

public class ReviewUser
{
    public string? FullName { get; set; }
}
