using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace classroom_monitoring_system.Controllers
{
    public class FingerprintController : Controller
    {
        private readonly IBaseRepository<User> _user;
        private readonly IBaseRepository<UserFingerprint> _userFingerprint;
        private readonly IBaseRepository<Room> _roomRepository;
        private readonly MonitorDbContext _context;
        public FingerprintController(
            IBaseRepository<User> userRepository, 
            IBaseRepository<UserFingerprint> userFingerprint,
            IBaseRepository<Room> roomRepository,
            MonitorDbContext context)
        {
            _user = userRepository;
            _userFingerprint = userFingerprint;
            _roomRepository = roomRepository;
            _context = context;
        }
        public IActionResult AddFingerPrint()
        {
            var editModel = new FingerprintScreenModel()
            {
                Users = _user
                    .GetByConditionAndIncludes(x => 
                        x.UserId != null, "UserRole", "UserFingerprints")
                    .Select(x => new
                    {
                        UserId = x.UserId,
                        FullName = x.FirstName + " " + x.LastName + " (" + x.UserFingerprints.Count() + " enrolled)"
                    })
                    .Cast<object>().ToList()
            };
            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterNew([FromBody] FingerprintRequest request)
        {
            if (request.PositionNumber == null || request.UserId == null)
            {
                return Json(new { isSuccessful = false, message = "Invalid fingerprint." });
            }

            var fingerprint = _userFingerprint.GetByCondition(x => x.PositionNumber == request.PositionNumber);
            if (fingerprint.Any())
            {
                return Json(new { isSuccessful = false, message = "Fingerprint is already enrolled." });
            }

            var newFingerprint = new UserFingerprint
            {
                UserId = Guid.Parse(request.UserId),
                PositionNumber = request.PositionNumber
            };
            var result = _userFingerprint.Save(newFingerprint);

            return Json(new { isSuccessful = true, message = "Succefully Saved" });
        }

        public IActionResult CheckRoom()
        {
            var editModel = new FingerprintScreenModel()
            {
                Users = _user
                    .GetByConditionAndIncludes(x =>
                        x.UserId != null, "UserRole", "UserFingerprints")
                    .Select(x => new
                    {
                        UserId = x.UserId,
                        FullName = x.FirstName + " " + x.LastName + " (" + x.UserFingerprints.Count() + " enrolled)"
                    })
                    .Cast<object>().ToList(),
                Rooms = _roomRepository.GetAll().Cast<object>().ToList()
            };
            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> CheckAssignmentLoginUsingDevice([FromBody] CheckAssignmentRequest request)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            var nowInManila = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var currentTime = TimeOnly.FromDateTime(nowInManila);
            var currentDate = DateOnly.FromDateTime(nowInManila);

            var assignment = _context.RoomSchedules
                .Include(r => r.ProfessorUser)
                .Include(r => r.ProfessorUser.UserFingerprints)
                .AsEnumerable() // <--- Move the rest of the query to memory
                .Where(r =>
                    r.RoomId == Guid.Parse(request.RoomId) &&
                    r.ProfessorUser?.UserFingerprints.Any(f => f.PositionNumber == request.PositionNumber) == true &&
                    r.StartTime <= currentTime &&
                    r.EndTime >= currentTime &&
                    r.DateOfUse == currentDate
                )
                .FirstOrDefault();

            var prof = _context.Users.FirstOrDefault(x => x.UserFingerprints.Any(f => f.PositionNumber == request.PositionNumber));

            var attendance = new Attendance
            {
                ProfessorId = prof.UserId,
                IsCorrectRoom = assignment != null,
                DateOfUse = currentDate,
                StartTime = currentTime,
                CreatedDate = DateTime.Now,
                RoomScheduleId = assignment != null ? assignment.RoomScheduleId : null
            };
            _context.Attendances.Add(attendance);
            _context.SaveChanges();


            if (assignment == null)
            {
                return Json(new { isSuccessful = false, message = "Professor is not assigned to the room" });
            }
            return Json(new { isSuccessful = true, message = "Professor is in the correct room" });
        }

        public class FingerprintRequest
        {
            public int PositionNumber { get; set; }
            public string UserId { get; set; }
        }
        public class CheckAssignmentRequest
        {
            public int PositionNumber { get; set; }
            public string RoomId { get; set; }
        }
    }
}
