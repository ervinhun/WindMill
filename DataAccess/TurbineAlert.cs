using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class TurbineAlert
{
    public long Id { get; set; }

    public string TurbineId { get; set; } = null!;

    public Guid FarmId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Severity { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
