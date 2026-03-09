public class Store// (Магазины)
{
   public int Id{get;set;}

    public string Name{get;set;}=null!;

    public string Address{get;set;}=null!;

    public string City{get;set;}=null!;

    public string Phone { get; set; } = string.Empty;

    // human-readable working hours or schedule (e.g. "Mon–Fri 09:00‑21:00")
    public string WorkingHours { get; set; } = string.Empty;
}