namespace classroom_monitoring_system.Models
{
    public class FingerprintScreenModel
    {
        public List<object> Users { get; set; }
        public Guid SelectedUser { get; set; }
        public List<object> Rooms { get; set; }
        public Guid SelectedRoome { get; set; }
    }
}
