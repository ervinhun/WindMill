using System;
namespace WindMill.Dto1;
public class TurbineSettingsHistoryFullDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Action { get; set; } = null!;
    public string? Settings { get; set; }
    public DateTime? CreatedAt { get; set; }
}
public class CreateTurbineSettingsHistoryDto
{
    public string TurbineId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Action { get; set; } = null!;
    public string? Settings { get; set; }
}
public class TurbineSettingsHistoryQueryDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Action { get; set; } = null!;
    public string? Settings { get; set; }
    public DateTime? CreatedAt { get; set; }
}
