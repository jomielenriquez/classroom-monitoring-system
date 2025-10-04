using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class RoomSchedule
{
    public Guid RoomScheduleId { get; set; }

    public Guid ProfessorUserId { get; set; }

    public Guid RoomId { get; set; }

    public DateOnly DateOfUse { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Note { get; set; }

    public virtual User ProfessorUser { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
