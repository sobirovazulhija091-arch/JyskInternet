public class FilterOrder
{
    public string? UserId { get; set; }               // Orders of specific user
    public EnumStatus? Status { get; set; }       // Pending / Delivered
    public DateTime? FromDate { get; set; }        // OrderDate >=
    public DateTime? ToDate { get; set; }          // OrderDate <=
    public decimal? MinTotalAmount { get; set; }   // Total >=
    public decimal? MaxTotalAmount { get; set; }   // Total <=
}
