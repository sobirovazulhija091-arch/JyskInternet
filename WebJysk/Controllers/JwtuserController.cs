using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/auth")]
public class JwtUserController( UserManager<User> userManager,SignInManager<User> 
signInManager,IJwtService jwt,IEmailService emailService) : ControllerBase
{
    private readonly UserManager<User> _userManager= userManager;
    private readonly SignInManager<User> _signInManager= signInManager;
    private readonly IJwtService _jwt= jwt;
    private readonly IEmailService _emailService= emailService; 

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest(new { message = "Email already exists" });

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            Phone = dto.Phone
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(user, "User");
        var token = await _jwt.CreateTokenAsync(user);
        await _emailService.SendAsync(dto.Email, "Welcome to JYSK", "You have successfully registered!");
        return Ok(new { token });
    }

   [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] JsonElement? body)
{
    LoginDto? dto = null;
    if (body.HasValue && body.Value.ValueKind != JsonValueKind.Null && body.Value.ValueKind != JsonValueKind.Undefined)
    {
        try
        {
            dto = JsonSerializer.Deserialize<LoginDto>(body.Value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch { }
    }
    if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        return BadRequest(new { message = "Email and password are required." });

    var user = await _userManager.FindByEmailAsync(dto.Email);
    if (user is null)
        return Unauthorized(new { message = "Invalid credentials" });

    var check = await _signInManager.CheckPasswordSignInAsync(
        user, dto.Password, lockoutOnFailure: true);

    if (!check.Succeeded)
    {
        return Unauthorized(new
        {
            message = check.IsLockedOut ? "Locked out (too many attempts)" : "Invalid credentials"
        });
    }

    var token = await _jwt.CreateTokenAsync(user);

    return Ok(new { token });
}


    [HttpPost("logout")]
    public IActionResult Logout()
        => Ok(new { message = "JWT logout: delete token on client" });

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Ok(new { name = User.Identity?.Name, email = (string?)null, phone = (string?)null, claims = User.Claims.Select(c => new { c.Type, c.Value }) });

        var user = await _userManager.FindByIdAsync(userId);
        return Ok(new
        {
            name = user?.FullName ?? User.Identity?.Name,
            email = user?.Email,
            phone = user?.Phone,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
