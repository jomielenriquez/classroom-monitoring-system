namespace classroom_monitoring_system.Models
{
    public class RoomScheduleSearchModel
    {
        public DateOnly DateOfUse { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
        public string RoomCode { get; set; }
    }
}
