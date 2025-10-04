using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace classroom_monitoring_system.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IBaseRepository<RoomType> _roomTypeRepository;
        private readonly RoomTypeRepository _roomTypeService;
        public RoomTypeController(IBaseRepository<RoomType> roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
            _roomTypeService = new RoomTypeRepository(_roomTypeRepository);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoomTypeListScreen(PageModel pageModel)
        {
            var result = _roomTypeService.GetList(pageModel);
            return View(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RoomTypeSearch(RoomTypeSearchModel roomTypeSearchModel)
        {
            var pageModel = new PageModel();
            pageModel.Search = JsonConvert.SerializeObject(roomTypeSearchModel);

            return RedirectToAction("RoomTypeListScreen", "RoomType", pageModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public int DeleteRoomType([FromBody] Guid[] selected)
        {
            var result = _roomTypeService.DeleteRoomType(selected);
            return result.DeleteCount;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoomTypeEdit(RoomType roomType)
        {
            var result = _roomTypeService.GetRoomTypeById(roomType.RoomTypeId);
            var editScreen = new EditScreenModel<RoomType>()
            {
                Data = result.Data
            };
            if (roomType.RoomTypeId != Guid.Empty)
            {
                return View(editScreen);
            }

            return View(editScreen);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Save(RoomType roomType)
        {
            var result = _roomTypeService.SaveRoomType(roomType);
            if (!result.IsSuccess)
            {
                var editScreen = new EditScreenModel<RoomType>()
                {
                    Data = result.Data,
                    ErrorMessages = result.Errors ?? new List<string> { }
                };
                return View("RoomTypeEdit", editScreen);
            }

            return RedirectToAction("RoomTypeListScreen", "RoomType");
        }
    }
}
