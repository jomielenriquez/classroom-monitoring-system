using Microsoft.AspNetCore.Mvc;

namespace classroom_monitoring_system.Controllers
{
    public class DeviceController : Controller
    {
        public IActionResult Login(string passGuid)
        {
            if (passGuid != "30F32AF3-7AA7-49D2-A227-7A3D06401EF4")
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
