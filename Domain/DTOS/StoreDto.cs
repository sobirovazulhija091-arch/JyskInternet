public class StoreDto
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Phone { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty; // e.g. "Mon–Fri 09:00‑21:00"
}