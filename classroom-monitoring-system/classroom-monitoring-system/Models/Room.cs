using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class Room
{
    public Guid RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public string RoomDescription { get; set; } = null!;

    public Guid RoomTypeId { get; set; }

    public string RoomCode { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<RoomSchedule> RoomSchedules { get; set; } = new List<RoomSchedule>();

    public virtual RoomType RoomType { get; set; } = null!;
}
