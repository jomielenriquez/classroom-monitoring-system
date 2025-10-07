using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace classroom_monitoring_system.Controllers
{
    public class RoomScheduleController : Controller
    {
        private readonly RoomScheduleRepository _roomScheduleService;
        private readonly IBaseRepository<Room> _roomRespository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IBaseRepository<RoomSchedule> _roomSchedule;
        private readonly IBaseRepository<Subject> _subjectRepository;
        public RoomScheduleController(IBaseRepository<RoomSchedule> roomScheduleRepository,
            IBaseRepository<Room> roomRespository, IBaseRepository<User> userRepository,
            IBaseRepository<UserRole> userRoleRepository, 
            IBaseRepository<Subject> subjectRepository)
        {
            _roomScheduleService = new RoomScheduleRepository(roomScheduleRepository);
            _roomRespository = roomRespository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roomSchedule = roomScheduleRepository;
            _subjectRepository = subjectRepository;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RoomScheduleListScreen(PageModel pageModel)
        {
            var result = _roomScheduleService.GetList(pageModel);
            return View(result.Data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RoomScheduleSearch(RoomScheduleSearchModel roomScheduleSearchModel)
        {
            var pageModel = new PageModel();
            pageModel.Search = JsonConvert.SerializeObject(roomScheduleSearchModel);

            return RedirectToAction("RoomScheduleListScreen", "RoomSchedule", pageModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public int DeleteRoomSchedule([FromBody] Guid[] selected)
        {
            var result = _roomScheduleService.DeleteRoomSchedule(selected);
            return result.DeleteCount;
        }
        // GET: api/roomschedules?roomId=123&start=2025-10-04&end=2025-10-05
        [HttpGet]
        public IActionResult GetSchedules(Guid roomId)
        {
            var scheds = roomId != Guid.Empty 
                ? _roomSchedule.GetByConditionAndIncludes(x =>
                    x.RoomId == roomId, "ProfessorUser", "Room") 
                : _roomSchedule.GetByConditionAndIncludes(x =>
                    x.RoomScheduleId != null, "ProfessorUser", "Room");

            var schedules = scheds
                .Select(r => new
                {
                    id = r.RoomScheduleId,
                    title = r.ProfessorUser != null ? r.ProfessorUser.FirstName + " " + r.ProfessorUser.LastName : "Reserved",
                    start = r.DateOfUse.ToDateTime(r.StartTime),
                    end = r.DateOfUse.ToDateTime(r.EndTime),
                    note = r.Note
                })
                .ToList();

            return Ok(schedules);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RoomScheduleEdit(RoomSchedule roomSchedule)
        {
            var result = _roomScheduleService.GetRoomById(roomSchedule.RoomScheduleId);
            var professorRole = _userRoleRepository.GetByCondition(x => x.RoleName == "Professor").FirstOrDefault();
            var editScreen = new EditScreenModel<RoomSchedule>()
            {
                Data = result.Data,
                Room = _roomRespository.GetAll().Cast<object>().ToList(),
                Subject = _subjectRepository.GetAll().Cast<object>().ToList(),
                Users = _userRepository
                    .GetByConditionAndIncludes(x => 
                        x.UserRole.UserRoleId == professorRole.UserRoleId)
                    .Select(x => new 
                    { 
                        UserId = x.UserId, 
                        FullName = x.FirstName + " " + x.LastName
                    })
                    .Cast<object>().ToList()

            };
            if (roomSchedule.RoomScheduleId != Guid.Empty)
            {
                return View(editScreen);
            }

            return View(editScreen);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Save(RoomSchedule roomSchedule)
        {
            var result = _roomScheduleService.SaveRoomSchedule(roomSchedule);
            if (!result.IsSuccess)
            {
                var professorRole = _userRoleRepository.GetByCondition(x => x.RoleName == "Professor").FirstOrDefault();
                var editScreen = new EditScreenModel<RoomSchedule>()
                {
                    Data = result.Data,
                    Room = _roomRespository.GetAll().Cast<object>().ToList(),
                    Subject = _subjectRepository.GetAll().Cast<object>().ToList(),
                    Users = _userRepository
                        .GetByConditionAndIncludes(x =>
                            x.UserRole.UserRoleId == professorRole.UserRoleId)
                        .Select(x => new
                        {
                            UserId = x.UserId,
                            FullName = x.FirstName + " " + x.LastName
                        })
                        .Cast<object>().ToList(),
                    ErrorMessages = result.Errors ?? new List<string> { }
                };
                return View("RoomScheduleEdit", editScreen);
            }

            return RedirectToAction("RoomScheduleListScreen", "RoomSchedule");
        }
    }
}
