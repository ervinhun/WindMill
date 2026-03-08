using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class TurbineAlert
{
    public long Id { get; set; }

    public Guid TurbineId { get; set; }

    public Guid FarmId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Severity { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Farm Farm { get; set; } = null!;

    public virtual Turbine Turbine { get; set; } = null!;
}
