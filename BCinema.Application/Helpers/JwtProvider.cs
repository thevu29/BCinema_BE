using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCinema.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BCinema.Application.Helpers;

public class JwtProvider(IConfiguration configuration)
{
    private readonly string? _key = configuration["JwtConfig:Secret"];
    private readonly string? _issuer = configuration["JwtConfig:Issuer"];
    private readonly string? _audience = configuration["JwtConfig:Audience"];

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key!);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("name", user.Name),
                new Claim("image", user.Avatar ?? ""),
                new Claim("point", user.Point.ToString() ?? "0")
            ]),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _issuer,
            Audience = _audience
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
    
    public static string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}