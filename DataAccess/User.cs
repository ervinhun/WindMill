using System;
using System.Collections.Generic;

namespace WindMill.DataAccess;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public Guid RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TurbineCommandHistory> TurbineCommandHistories { get; set; } = new List<TurbineCommandHistory>();

    public virtual ICollection<TurbineSettingsHistory> TurbineSettingsHistories { get; set; } = new List<TurbineSettingsHistory>();
}
