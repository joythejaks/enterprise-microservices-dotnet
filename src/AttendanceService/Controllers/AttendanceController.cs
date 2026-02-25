using AttendanceService.Data;
using AttendanceService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly AttendanceDbContext _context;

    public AttendanceController(AttendanceDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("check-in")]
    public IActionResult CheckIn()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(username))
            return Unauthorized("Invalid token.");

        var record = new AttendanceRecord
        {
            Id = Guid.NewGuid(),
            Username = username,
            CheckInTime = DateTime.UtcNow
        };

        _context.AttendanceRecords.Add(record);
        _context.SaveChanges();

        return Ok(new
        {
            message = "Check-in successful",
            user = username,
            role = role,
            time = record.CheckInTime
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("check-in-admin")]
    public IActionResult CheckInAdmin()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "Check-in successful",
            user = username,
            role = role,
            time = DateTime.UtcNow
        });
    }
}