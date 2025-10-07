using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Mvc;

namespace classroom_monitoring_system.Controllers
{
    public class FingerprintController : Controller
    {
        private readonly IBaseRepository<User> _user;
        private readonly IBaseRepository<UserFingerprint> _userFingerprint;
        public FingerprintController(IBaseRepository<User> userRepository, IBaseRepository<UserFingerprint> userFingerprint)
        {
            _user = userRepository;
            _userFingerprint = userFingerprint;
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

        public class FingerprintRequest
        {
            public int PositionNumber { get; set; }
            public string UserId { get; set; }
        }
    }
}
