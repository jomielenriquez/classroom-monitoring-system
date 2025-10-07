using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace classroom_monitoring_system.Controllers
{
    public class LoginController : Controller
    {
        private readonly MonitorDbContext _context;

        public LoginController(MonitorDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var pass = LoginRepository.ComputeMd5Hash(password);
            var user = _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefault(u => u.UserName == username && u.Password == pass);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.RoleName) // Update to have different roles
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                // Successful login
                // You can set session or authentication cookie here
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Invalid credentials
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> LoginUsingDevice(int positionNumber)
        {
            var fingerprint = _context.UserFingerprints
                .Include(uf => uf.User)
                .ThenInclude(u => u.UserRole)
                .FirstOrDefault(uf => uf.PositionNumber == positionNumber);

            if (fingerprint != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, fingerprint.User.FirstName + " " + fingerprint.User.LastName),
                    new Claim("Id", fingerprint.User.UserId.ToString()),
                    new Claim(ClaimTypes.Role, fingerprint.User.UserRole.RoleName), // Update to have different roles
                    new Claim("passGuid", DeviceController.passGuid),
                    new Claim("IsFromDevice", "true"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                // Successful login
                // You can set session or authentication cookie here
                return Json(new { isSuccessful = true, redirectUrl = Url.Action("Dashboard", "Device") });
            }
            else
            {
                // Invalid credentials
                ViewBag.ErrorMessage = "Invalid username or password.";
                return Json(new { isSuccessful = false, message = "Invalid fingerprint." });
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
        [Authorize]
        public async Task<IActionResult> LogoutUsingDevice()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Device");
        }
    }
}
