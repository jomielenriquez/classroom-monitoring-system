using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace classroom_monitoring_system.Controllers
{
    public class AttendanceController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult AttendanceListScreen()
        {
            return RedirectToAction("Index","Home");
        }
    }
}
