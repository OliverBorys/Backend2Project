using Backend2Project.Dtos;
using Backend2Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend2Project.Controllers;

[Route("api/auth")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly AuthService authService;
    private readonly IConfiguration configuration;

    public AuthController(AuthService authService, IConfiguration configuration)
    {
        this.authService = authService;
        this.configuration = configuration;
    }

    [HttpPost("token")]
    public ActionResult CreateToken(CredentialsDto credentialsDto)
    {
        var user = authService.Authenticate(
            credentialsDto.Username,
            credentialsDto.Password);

        if (user is null)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var lifetime = TimeSpan.FromHours(1);
        var now = DateTime.UtcNow;

        var token = new JwtSecurityToken(
            claims: claims,
            expires: now.Add(lifetime),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new TokenResponseDto
        {
            access_token = accessToken,
            token_type = "Bearer",
            expires_in = (int)lifetime.TotalSeconds
        });
    }
}