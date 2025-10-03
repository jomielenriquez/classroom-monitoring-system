using System.Diagnostics;
using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace classroom_monitoring_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseRepository<User> _userRepository;

        public HomeController(ILogger<HomeController> logger, IBaseRepository<User> user)
        {
            _logger = logger;
            _userRepository = user;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAll().ToList();
            return View(users);
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
