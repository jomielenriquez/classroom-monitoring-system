using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class UserRole
{
    public Guid UserRoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
