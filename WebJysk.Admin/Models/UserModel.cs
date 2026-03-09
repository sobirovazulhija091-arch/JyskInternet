namespace WebJysk.Admin.Models;

public class UserModel
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string FullName { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public class FilterUser
{
    public string? Email { get; set; }
    public string? FullName { get; set; }
}
