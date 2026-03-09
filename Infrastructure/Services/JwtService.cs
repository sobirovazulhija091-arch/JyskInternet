
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtService(UserManager<User> userManager, IConfiguration configuration) : IJwtService
{
    private readonly UserManager<User> _user = userManager;
      private readonly IConfiguration config = configuration;
    
     public async Task<string> CreateTokenAsync(User user)
    {
        var jwt = config.GetSection("jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var userClaims = await _user.GetClaimsAsync(user); // ✅ ADD THIS

        var roles = await _user.GetRolesAsync(user);

        // Claims = данные, которые попадут в токен
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.Name, user.FullName ?? user.PhoneNumber ?? user.UserName ?? user.Email ?? user.Id  ),
        };
        claims.AddRange(userClaims);

        // роли тоже можно положить в токен
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}