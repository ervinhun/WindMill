using System;

namespace WindMill.Dto1;

public class TurbineAlertFullDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public Guid FarmId { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Severity { get; set; }
    public string Message { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}

public class CreateTurbineAlertDto
{
    public string TurbineId { get; set; } = null!;
    public Guid FarmId { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Severity { get; set; }
    public string Message { get; set; } = null!;
}

public class TurbineAlertQueryDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public Guid FarmId { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Severity { get; set; }
    public string Message { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}

