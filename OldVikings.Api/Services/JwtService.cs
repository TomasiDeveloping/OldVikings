using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OldVikings.Api.Classes;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Services;

public class JwtService(IOptions<JwtSettings> optionsMonitor) : IJwtService
{
    private readonly JwtSettings _jwtSettings = optionsMonitor.Value;
    public string GenerateTokenAsync(string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credential);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}