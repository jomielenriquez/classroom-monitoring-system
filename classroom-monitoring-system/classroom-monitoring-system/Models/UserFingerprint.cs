using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class UserFingerprint
{
    public Guid UserFingerprintId { get; set; }

    public Guid UserId { get; set; }

    public int PositionNumber { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}
