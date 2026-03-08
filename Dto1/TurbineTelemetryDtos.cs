using System;

namespace WindMill.Dto1;

public class TurbineTelemetryFullDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public double? WindSpeed { get; set; }
    public double? PowerOutput { get; set; }
    public double? NacelleDirection { get; set; }
    public double? BladePitch { get; set; }
    public double? Vibration { get; set; }
    public bool IsRunning { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateTurbineTelemetryDto
{
    public string TurbineId { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public double? WindSpeed { get; set; }
    public double? PowerOutput { get; set; }
    public double? NacelleDirection { get; set; }
    public double? BladePitch { get; set; }
    public double? Vibration { get; set; }
    public bool IsRunning { get; set; }
}

public class TurbineTelemetryQueryDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public double? WindSpeed { get; set; }
    public double? PowerOutput { get; set; }
    public double? NacelleDirection { get; set; }
    public double? BladePitch { get; set; }
    public double? Vibration { get; set; }
    public bool IsRunning { get; set; }
    public DateTime? CreatedAt { get; set; }
}
