using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class EmailController(IEmailService service, UserManager<User> userManager, IConfiguration config) : ControllerBase
    {
          private readonly IEmailService _emailService = service;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailService.SendAsync("sobirovazulhija091@gmail.com",
            "WELLCOME TO JYSK INTERNET SHOP",
            "Test email from WebJysk API");
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("add-permission")]
        public async Task<IActionResult> AddPermissionToUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddClaimAsync(user,
                new Claim("Permission", "SendEmail"));
            return Ok();
        }

        [Authorize]
        [HttpGet("debug/claims")]
        public IActionResult DebugClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Ok();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontend = config["FrontendBaseUrl"] ?? "http://localhost:5211";
            var link = $"{frontend}/reset-password?email={Uri.EscapeDataString(dto.Email)}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendAsync(dto.Email,
                "Reset Password - JYSK",
                $"Click the link below to reset your password:\n\n{link}\n\nThis link expires in 24 hours. If you did not request this, ignore this email.");

            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                dto.Token,
                dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
