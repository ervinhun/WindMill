using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class TurbineSettingsHistory
{
    public long Id { get; set; }

    public Guid TurbineId { get; set; }

    public Guid UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? Settings { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Turbine Turbine { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
