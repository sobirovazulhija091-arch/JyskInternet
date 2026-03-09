using Microsoft.AspNetCore.Identity;

public class UpdateUserDto
{
    public Guid Id { get; set; } 
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone{get;set;}
    public string Password { get; set; } = null!;
}
