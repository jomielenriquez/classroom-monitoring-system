using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace classroom_monitoring_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseRepository<User> _userRepository;
        private readonly MonitorDbContext _context;

        public HomeController(ILogger<HomeController> logger, IBaseRepository<User> user, MonitorDbContext context)
        {
            _logger = logger;
            _userRepository = user;
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var dashboard = new DashboardModel();

            dashboard.UserWithNoFingerprint = _context.Users
                .Where(u => !_context.UserFingerprints.Any(uf => uf.UserId == u.UserId))
                .ToList();

            var users = _userRepository.GetAll().ToList();
            return View(users);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetWeeklySummary()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var weekStart = today.AddDays(-6); // last 7 days

            var attendanceData = _context.Attendances
                .Where(a => a.DateOfUse >= weekStart && a.DateOfUse <= today)
                .GroupBy(a => a.DateOfUse)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            var labels = attendanceData.Select(d => (d.Date ?? DateOnly.FromDateTime(DateTime.Now)).ToString("MMM dd")).ToList();
            var values = attendanceData.Select(d => d.Count).ToList();

            return Json(new { labels, values });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
