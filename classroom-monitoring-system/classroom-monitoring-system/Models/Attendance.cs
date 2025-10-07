using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class Attendance
{
    public Guid AttendanceId { get; set; }

    public Guid ProfessorId { get; set; }

    public Guid? RoomScheduleId { get; set; }

    public bool IsCorrectRoom { get; set; }

    public Guid? RoomReassignmentId { get; set; }

    public DateOnly? DateOfUse { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User Professor { get; set; } = null!;

    public virtual Room? RoomReassignment { get; set; }

    public virtual RoomSchedule? RoomSchedule { get; set; }
}
