using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Mvc;

namespace classroom_monitoring_system.Controllers
{
    public class FingerprintController : Controller
    {
        private readonly IBaseRepository<User> _user;
        public FingerprintController(IBaseRepository<User> userRepository)
        {
            _user = userRepository;
        }
        public IActionResult AddFingerPrint()
        {
            var editModel = new FingerprintScreenModel()
            {
                Users = _user
                    .GetByConditionAndIncludes(x => 
                        x.UserId != null, "UserRole")
                    .Select(x => new
                    {
                        UserId = x.UserId,
                        FullName = x.FirstName + " " + x.LastName
                    })
                    .Cast<object>().ToList()
            };
            return View(editModel);
        }
    }
}
