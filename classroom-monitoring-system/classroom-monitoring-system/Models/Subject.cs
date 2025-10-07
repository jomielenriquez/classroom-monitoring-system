using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class Subject
{
    public Guid SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public string SubjectDescription { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<RoomSchedule> RoomSchedules { get; set; } = new List<RoomSchedule>();
}
