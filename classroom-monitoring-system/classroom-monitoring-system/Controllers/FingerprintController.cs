using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace classroom_monitoring_system.Controllers
{
    public class FingerprintController : Controller
    {
        private readonly IBaseRepository<User> _user;
        private readonly IBaseRepository<UserFingerprint> _userFingerprint;
        private readonly IBaseRepository<Room> _roomRepository;
        private readonly IBaseRepository<Attendance> _attendance;
        private readonly IBaseRepository<Subject> _subject;
        private readonly IBaseRepository<RoomSchedule> _roomSchedule;
        private readonly MonitorDbContext _context;
        public FingerprintController(
            IBaseRepository<User> userRepository, 
            IBaseRepository<UserFingerprint> userFingerprint,
            IBaseRepository<Room> roomRepository,
            MonitorDbContext context,
            IBaseRepository<Attendance> attendance,
            IBaseRepository<Subject> subject,
            IBaseRepository<RoomSchedule> roomSchedule)
        {
            _user = userRepository;
            _userFingerprint = userFingerprint;
            _roomRepository = roomRepository;
            _context = context;
            _attendance = attendance;
            _subject = subject;
            _roomSchedule = roomSchedule;
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

            string message = "Professor is in the correct room";
            var isSuccessful = true;
            if (assignment == null)
            {
                isSuccessful = false;
                message = "Professor is not assigned to the room";
            }
            return Json(new { 
                isSuccessful = isSuccessful, 
                attendanceId = attendance.AttendanceId, 
                message = message });
        }
        [Authorize]
        public IActionResult GetTime(string attendanceId)
        {
            var attendance = _attendance
                .GetByConditionAndIncludes(
                    x => x.AttendanceId == Guid.Parse(attendanceId), 
                    "Professor")
                .FirstOrDefault();
            return View(attendance);
        }
        [Authorize]
        public ActionResult SaveTime(Attendance attendance)
        {
            var existingAttendance = _attendance.GetById(attendance.AttendanceId);
            if (existingAttendance != null)
            {
                existingAttendance.StartTime = attendance.StartTime;
                existingAttendance.EndTime = attendance.EndTime;
                _attendance.Update(existingAttendance);
                return RedirectToAction("AvailableRoom", attendance);
            }
            return RedirectToAction("CheckRoom");
        }
        [Authorize]
        public IActionResult AvailableRoom(Attendance attendance)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            var nowInManila = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var currentTime = TimeOnly.FromDateTime(nowInManila);
            var currentDate = DateOnly.FromDateTime(nowInManila);

            var existingAttendance = _attendance
                .GetByConditionAndIncludes(a =>
                    a.AttendanceId == attendance.AttendanceId,
                    "Professor")
                .FirstOrDefault();

            var editModel = new FingerprintScreenModel()
            {
                Rooms = _roomRepository
                    .GetByConditionAndIncludes(x => 
                        !x.RoomSchedules.Any(s =>
                            s.StartTime <= currentTime &&
                            s.EndTime >= currentTime &&
                            s.DateOfUse == currentDate)).Cast<object>().ToList(),
                Attendance = existingAttendance
            };
            return View(editModel);
        }
        [Authorize]
        public ActionResult SaveRoom(Attendance attendance)
        {
            var existingAttendance = _attendance.GetById(attendance.AttendanceId);
            if (existingAttendance != null)
            {
                existingAttendance.RoomReassignmentId = attendance.RoomReassignmentId;
                _attendance.Update(existingAttendance);
                return RedirectToAction("SelectSubject", attendance);
            }
            return RedirectToAction("CheckRoom");
        }
        [Authorize]
        public IActionResult SelectSubject(Attendance attendance)
        {
            var existingAttendance = _attendance
                .GetByConditionAndIncludes(a => 
                    a.AttendanceId == attendance.AttendanceId,
                    "Professor")
                .FirstOrDefault();

            var editModel = new FingerprintScreenModel()
            {
                Subject = _subject.GetAll().Cast<object>().ToList(),
                Attendance = existingAttendance
            };
            return View(editModel);
        }
        [Authorize]
        public IActionResult SaveSubject(SubjectRequest subjectRequest)
        {
            var existingAttendance = _attendance
                .GetByConditionAndIncludes(a =>
                    a.AttendanceId == subjectRequest.AttendanceId,
                    "Professor")
                .FirstOrDefault();

            if (existingAttendance != null)
            {
                var now = DateTime.Now;
                RoomSchedule roomSchedule = new RoomSchedule
                {
                    ProfessorUserId = existingAttendance.ProfessorId,
                    RoomId = existingAttendance.RoomReassignmentId.Value,
                    SubjectId = subjectRequest.SubjectId,
                    DateOfUse = existingAttendance.DateOfUse ?? DateOnly.FromDateTime(now),
                    StartTime = existingAttendance.StartTime ?? TimeOnly.FromDateTime(now),
                    EndTime = existingAttendance.EndTime ?? TimeOnly.FromDateTime(now),
                    CreatedDate = DateTime.Now
                };

                var data = _roomSchedule.Save(roomSchedule);

                existingAttendance.RoomScheduleId = data.RoomScheduleId;

                _attendance.Update(existingAttendance);
                return RedirectToAction("Success", existingAttendance);
            }
            return RedirectToAction("CheckRoom");
        }
        [Authorize]
        public IActionResult Success(Attendance attendance)
        {
            var existingAttendance = _context.Attendances
                .Include(x => x.RoomSchedule)
                .Include(x => x.RoomSchedule.Subject)
                .Include(x => x.RoomSchedule.Room)
                .Include(x => x.Professor)
                .Include(x => x.RoomReassignment)
                .FirstOrDefault(a => a.AttendanceId == attendance.AttendanceId);

            return View(existingAttendance);
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
        public class SubjectRequest
        {
            public Guid SubjectId { get; set; }
            public Guid AttendanceId { get; set; }
        }
    }
}
