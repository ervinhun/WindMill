using System;
namespace WindMill.Dto1;
public class TurbineCommandHistoryFullDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Action { get; set; } = null!;
    public int? Value { get; set; }
    public double? Angle { get; set; }
    public string? Reason { get; set; }
    public DateTime? CreatedAt { get; set; }
}
public class CreateTurbineCommandHistoryDto
{
    public string TurbineId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Action { get; set; } = null!;
    public int? Value { get; set; }
    public double? Angle { get; set; }
    public string? Reason { get; set; }
}
public class TurbineCommandHistoryQueryDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Action { get; set; } = null!;
    public int? Value { get; set; }
    public double? Angle { get; set; }
    public string? Reason { get; set; }
    public DateTime? CreatedAt { get; set; }
}
