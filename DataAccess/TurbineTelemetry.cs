using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class TurbineTelemetry
{
    public long Id { get; set; }

    public Guid TurbineId { get; set; }

    public DateTime Timestamp { get; set; }

    public double? WindSpeed { get; set; }

    public double? WindDirection { get; set; }

    public double? AmbientTemperature { get; set; }

    public double? RotorSpeed { get; set; }

    public double? PowerOutput { get; set; }

    public double? NacelleDirection { get; set; }

    public double? BladePitch { get; set; }

    public double? GeneratorTemp { get; set; }

    public double? GearboxTemp { get; set; }

    public double? Vibration { get; set; }

    public bool IsRunning { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Turbine Turbine { get; set; } = null!;
}
