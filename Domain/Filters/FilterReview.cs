public class FilterReview
{
    public int? ProductId { get; set; }      // Reviews of specific product
    public string? UserId { get; set; }      // Reviews of specific user
    public int? Rating { get; set; }         // Filter by stars (1–5)
    public DateTime? FromDate { get; set; }  // Reviews from date
    public DateTime? ToDate { get; set; }    // Reviews to date
}