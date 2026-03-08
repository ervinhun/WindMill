using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class Farm
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TurbineAlert> TurbineAlerts { get; set; } = new List<TurbineAlert>();

    public virtual ICollection<Turbine> Turbines { get; set; } = new List<Turbine>();
}
