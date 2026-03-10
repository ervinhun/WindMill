using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class TurbineAlert
{
    public long Id { get; set; }

    public string TurbineId { get; set; } = null!;

    public string? FarmId { get; set; }

    public DateTime Timestamp { get; set; }

    public string Severity { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
