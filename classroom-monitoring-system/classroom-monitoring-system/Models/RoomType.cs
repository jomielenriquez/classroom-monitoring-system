using System;
using System.Collections.Generic;

namespace classroom_monitoring_system.Models;

public partial class RoomType
{
    public Guid RoomTypeId { get; set; }

    public string RoomTypeName { get; set; } = null!;

    public string RoomTypeDescription { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
