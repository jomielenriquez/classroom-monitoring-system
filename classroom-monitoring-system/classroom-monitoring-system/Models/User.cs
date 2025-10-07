using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid UserRoleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<RoomSchedule> RoomSchedules { get; set; } = new List<RoomSchedule>();

    public virtual ICollection<UserFingerprint> UserFingerprints { get; set; } = new List<UserFingerprint>();

    public virtual UserRole UserRole { get; set; } = null!;
}
