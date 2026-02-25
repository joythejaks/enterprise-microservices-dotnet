using Microsoft.AspNetCore.Mvc;
using AuthService.Data;
using AuthService.Entities;
using AuthService.DTOs;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AuthService.Services;
using AuthService.Common;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AuthDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exists = await _context.Users
            .AnyAsync(u => u.Username == request.Username);

        if (exists)
            return BadRequest(ApiResponse<string>.FailResponse(
                "Username already exists"
            ));

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.SuccessResponse(
            string.Empty,
            "User registered successfully"
        ));
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid username or password");

        var token = _jwtService.GenerateToken(user);

        return Ok(ApiResponse<LoginResponse>.SuccessResponse(
            new LoginResponse { Token = token },
            "Login successful"
        ));
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Username = username,
            Role = role
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok(ApiResponse<string>.SuccessResponse(
            "You are Admin!",
            "Access granted"
        ));
    }

    [HttpPost("bootstrap-admin")]
    public async Task<IActionResult> BootstrapAdmin(RegisterRequest request)
    {
        // 🔒 Only allow if environment variable is enabled
        if (!Environment.GetEnvironmentVariable("ALLOW_BOOTSTRAP")?.Equals("true") == true)
            return Unauthorized("Bootstrap not allowed.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var adminExists = await _context.Users
            .AnyAsync(u => u.Role == "Admin");

        if (adminExists)
            return BadRequest(ApiResponse<string>.FailResponse(
                "Admin already exists"
            ));

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "Admin"
        };

        _context.Users.Add(admin);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Admin created successfully." });
    }
}