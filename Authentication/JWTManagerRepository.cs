using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FormGeneratorAPI.Authentication.Entities;

namespace FormGeneratorAPI.Authentication;

public class JwtManagerRepository : IJwtManagerRepository
{
    private readonly IConfiguration _configuration;
    public JwtManagerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public JwtToken Authenticate(User user)
    {
        // Else we generate JSON Web Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtToken { Token = tokenHandler.WriteToken(token), Expires = token.ValidTo };
    }
}