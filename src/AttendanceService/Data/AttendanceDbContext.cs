using Microsoft.EntityFrameworkCore;
using AttendanceService.Entities;

namespace AttendanceService.Data;

public class AttendanceDbContext : DbContext
{
    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
}