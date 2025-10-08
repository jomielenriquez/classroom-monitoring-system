namespace classroom_monitoring_system.Models
{
    public class FingerprintScreenModel
    {
        public List<object> Users { get; set; }
        public Guid SelectedUser { get; set; }
        public List<object> Rooms { get; set; }
        public Guid SelectedRoom { get; set; }
        public List<object> Subject { get; set; }
        public Guid SelectedSubject { get; set; }
        public Attendance Attendance { get; set; }
    }
}
