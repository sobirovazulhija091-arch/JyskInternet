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
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        var token = await _jwt.CreateTokenAsync(user);
        await _emailService.SendAsync(dto.Email, "Registering to App", "You have successfully registered to our application!");
        await _userManager.AddToRoleAsync(user, "User");
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
    public IActionResult Me()
        => Ok(new
        {
            name = User.Identity?.Name,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
}
