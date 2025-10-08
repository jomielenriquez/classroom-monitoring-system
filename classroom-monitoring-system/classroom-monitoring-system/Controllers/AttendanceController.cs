using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using classroom_monitoring_system.Models;
using Microsoft.EntityFrameworkCore;

namespace classroom_monitoring_system.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly MonitorDbContext _dbContext;
        public AttendanceController(MonitorDbContext dbContext){
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin, Checker")]
        public IActionResult AttendanceListScreen()
        {
            var attendance = _dbContext
                .Attendances
                .Include(x => x.Professor)
                .Include(x => x.RoomSchedule)
                .Include(x => x.RoomSchedule.Subject)
                .Include(x => x.RoomSchedule.Room)
                .Include(x => x.RoomSchedule.ProfessorUser)
                .Include(x => x.RoomReassignment)
                .ToList();
            
            return View(attendance);
        }
    }
}
