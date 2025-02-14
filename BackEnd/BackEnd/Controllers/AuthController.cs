using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly WebDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthController(WebDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
    {
        // Validate the input
        if (string.IsNullOrWhiteSpace(signInModel.Email) || string.IsNullOrWhiteSpace(signInModel.Password))
        {
            return BadRequest("Email and password must be provided.");
        }

        // Check if the user exists in the database
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == signInModel.Email);

        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(signInModel.Password, existingUser.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        // Generate the JWT token
        var token = GenerateJwtToken(existingUser.Email);

        return Ok(new { token, id = existingUser.Id });
    }


    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(7).AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        // Validate the input
        if (string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
        {
            return BadRequest("Both current and new passwords must be provided.");
        }

        // Fetch the user from the database using the UserId from the model
        var user = await _context.Users.FindAsync(model.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Verify the current password
        if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
        {
            return BadRequest("The current password is incorrect.");
        }

        // Hash the new password and update the user record
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        user.UpdatedAt = DateTime.UtcNow.AddHours(7);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok("Password changed successfully.");
    }
    
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail(string email)
    {
        var apiKey = _configuration["API_KEY:Hunter"];
        var client = new HttpClient();
        var response = await client.GetAsync($"https://api.hunter.io/v2/email-verifier?email={email}&api_key={apiKey}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(500, "Error checking email.");
        }

        var result = await response.Content.ReadAsStringAsync();
        return Ok(result);
    }

}
