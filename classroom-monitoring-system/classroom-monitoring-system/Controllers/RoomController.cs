using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace classroom_monitoring_system.Controllers
{
    public class RoomController : Controller
    {
        private readonly RoomRepository _roomService;
        private readonly IBaseRepository<RoomType> _roomTypeRespository;
        public RoomController(IBaseRepository<Room> roomRepository, IBaseRepository<RoomType> roomTypeRepository)
        {
            _roomService = new RoomRepository(roomRepository);
            _roomTypeRespository = roomTypeRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoomListScreen(PageModel pageModel)
        {
            var result = _roomService.GetList(pageModel);
            return View(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RoomSearch(RoomSearchModel roomSearchModel)
        {
            var pageModel = new PageModel();
            pageModel.Search = JsonConvert.SerializeObject(roomSearchModel);

            return RedirectToAction("RoomListScreen", "Room", pageModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public int DeleteRoom([FromBody] Guid[] selected)
        {
            var result = _roomService.DeleteRoom(selected);
            return result.DeleteCount;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoomEdit(Room room)
        {
            var result = _roomService.GetRoomById(room.RoomId);
            var editScreen = new EditScreenModel<Room>()
            {
                Data = result.Data,
                RoomType = _roomTypeRespository.GetAll().Cast<object>().ToList()

            };
            if (room.RoomTypeId != Guid.Empty)
            {
                return View(editScreen);
            }

            return View(editScreen);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Save(Room room)
        {
            var result = _roomService.SaveRoom(room);
            if (!result.IsSuccess)
            {
                var editScreen = new EditScreenModel<Room>()
                {
                    Data = result.Data,
                    RoomType = _roomTypeRespository.GetAll().Cast<object>().ToList(),
                    ErrorMessages = result.Errors ?? new List<string> { }
                };
                return View("UserEdit", editScreen);
            }

            return RedirectToAction("RoomListScreen", "Room");
        }
    }
}
