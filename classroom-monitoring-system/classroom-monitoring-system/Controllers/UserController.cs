using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace classroom_monitoring_system.Controllers
{
    public class UserController : Controller
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly UserRepository _userService;

        public UserController(IBaseRepository<User> baseRepository, IBaseRepository<UserRole> userRoleRepository)
        {
            _userRepository = baseRepository;
            _userRoleRepository = userRoleRepository;
            _userService = new UserRepository(_userRepository, _userRoleRepository);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserListScreen(PageModel pageModel)
        {
            var result = _userService.GetList(pageModel);
            return View(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UserSearch(UserSearchModel userSearchModel)
        {
            var pageModel = new PageModel();
            pageModel.Search = JsonConvert.SerializeObject(userSearchModel);

            return RedirectToAction("UserListScreen", "User", pageModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public int DeleteUser([FromBody] Guid[] selected)
        {
            var result = _userService.DeleteUser(selected);
            return result.DeleteCount;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserEdit(User user)
        {
            var result = _userService.GetUserById(user.UserId);
            var editScreen = new EditScreenModel<User>()
            {
                Data = result.Data,
                UserRoles = _userRoleRepository.GetAll().Cast<object>().ToList()
            };
            if (user.UserId != Guid.Empty)
            {
                return View(editScreen);
            }

            return View(editScreen);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Save(User user)
        {
            var result = _userService.SaveUser(user);
            if (!result.IsSuccess)
            {
                var editScreen = new EditScreenModel<User>()
                {
                    Data = result.Data,
                    UserRoles = _userRoleRepository.GetAll().Cast<object>().ToList(),
                    ErrorMessages = result.Errors ?? new List<string> { }
                };
                return View("UserEdit", editScreen);
            }

            return RedirectToAction("UserListScreen", "User");
        }
    }
}
