public class FilterProduct
{
    public string? Name { get; set; }          // Search by product name (Contains)
    public int? CategoryId { get; set; }       // WHERE CategoryId
    public int? BrandId { get; set; }          // WHERE BrandId
    public decimal? MinPrice { get; set; }     // Price >= MinPrice
    public decimal? MaxPrice { get; set; }     // Price <= MaxPrice
    public bool? IsActive { get; set; }      //// Check do we have this product or not 
}