using classroom_monitoring_system.Models;
using classroom_monitoring_system.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace classroom_monitoring_system.Controllers
{
    public class EmailController : Controller
    {
        private readonly MonitorDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        public EmailController(MonitorDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _emailService = new EmailService(configuration);
        }

        [HttpPost]
        public IActionResult SendSchedule()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            var nowInManila = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var currentDate = DateOnly.FromDateTime(nowInManila);

            var professors = _dbContext.Users
                .Include(u => u.RoomSchedules)
                .Include("RoomSchedules.Room")
                .Include("RoomSchedules.Subject")
                .Where(u => 
                    u.UserRole.RoleName == "Professor" &&
                    u.RoomSchedules.Any(rs => rs.DateOfUse == currentDate)
                    )
                .ToList();

            foreach (var professor in professors)
            {
                var message = DraftSchedule(professor.RoomSchedules.Where(rs => rs.DateOfUse == currentDate).ToList());
                _emailService.SendEmail(
                    to: professor.Email,
                    subject: "Your Schedule for Today",
                    body: message
                );
            }

            return Ok(new { success = true, message = "Schedule email sent successfully." });
        }

        public string DraftSchedule(List<RoomSchedule> roomSchedule)
        {
            StringBuilder sb = new StringBuilder();
            if (roomSchedule.Any())
            {
                sb.AppendLine("Here is your schedule for today:<br/><br/>");
                sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
                sb.AppendLine("<tr><th>Time</th><th>Room</th><th>Subject</th><th>Note</th></tr>");
                foreach (var schedule in roomSchedule)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td>{schedule.StartTime.ToString("hh:mm tt")} - {schedule.EndTime.ToString("hh:mm tt")}</td>");
                    sb.AppendLine($"<td>{schedule.Room.RoomName} [{schedule.Room.RoomCode}]</td>");
                    sb.AppendLine($"<td>{schedule.Subject.SubjectName}</td>");
                    sb.AppendLine($"<td>{(string.IsNullOrEmpty(schedule.Note) ? "N/A" : schedule.Note)}</td>");
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table>");
                return sb.ToString();
            }
            else
            {
                return "You have no scheduled classes for today.";
            }
            return "";
        }
    }
}
