using Microsoft.AspNetCore.Mvc;

namespace classroom_monitoring_system.Controllers
{
    public class DeviceController : Controller
    {
        public static string passGuid = "30F32AF3-7AA7-49D2-A227-7A3D06401EF4";
        public IActionResult Login(string passGuid)
        {
            HttpContext.Session.SetString("IsFromDevice", "true");

            if (passGuid != passGuid)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
