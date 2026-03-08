using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class Turbine
{
    public Guid Id { get; set; }

    public string TurbineIdentifier { get; set; } = null!;

    public string TurbineName { get; set; } = null!;

    public Guid FarmId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Farm Farm { get; set; } = null!;

    public virtual ICollection<TurbineAlert> TurbineAlerts { get; set; } = new List<TurbineAlert>();

    public virtual ICollection<TurbineCommandHistory> TurbineCommandHistories { get; set; } = new List<TurbineCommandHistory>();

    public virtual ICollection<TurbineSettingsHistory> TurbineSettingsHistories { get; set; } = new List<TurbineSettingsHistory>();

    public virtual ICollection<TurbineTelemetry> TurbineTelemetries { get; set; } = new List<TurbineTelemetry>();
}
