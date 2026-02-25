namespace AttendanceService.Entities;

public class AttendanceRecord
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CheckInTime { get; set; }
}