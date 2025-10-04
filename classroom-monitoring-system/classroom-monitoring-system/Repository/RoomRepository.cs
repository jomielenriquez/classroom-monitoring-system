using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace classroom_monitoring_system.Repository
{
    public class RoomRepository
    {
        private readonly IBaseRepository<Room> _roomRepository;
        public RoomRepository(IBaseRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public Results<Room> GetRoomById(Guid roomId)
        {
            var result = new Results<Room>();
            result.IsSuccess = true;
            result.Data = _roomRepository.GetByCondition(
                x => x.RoomId == roomId).FirstOrDefault() ?? new Room()
                {
                    CreatedDate = DateTime.Now
                };
            return result;
        }
        private Results<List<string>> IsValid(Room room)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(room.RoomName))
            {
                errors.Add("Room Name is required.");
            }
            if (string.IsNullOrEmpty(room.RoomDescription))
            {
                errors.Add("Room description is required.");
            }
            return new Results<List<string>> { IsSuccess = !errors.Any(), Errors = errors };
        }
        public Results<Room> SaveRoom(Room room)
        {
            var result = new Results<Room>();
            result.IsSuccess = true;
            result.Data = room;
            result.Errors = new List<string>();
            var validationResult = IsValid(room);
            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Errors = validationResult.Errors;
                return result;
            }
            if (room.RoomId == Guid.Empty)
            {
                result.Data = _roomRepository.Save(room);
            }
            else
            {
                result.Data = _roomRepository.Update(room);
            }
            return result;
        }
        public Results<Room> DeleteRoom(Guid[] roomIds)
        {
            var result = new Results<Room>();
            result.IsSuccess = true;
            result.DeleteCount = 0;
            if (roomIds != null && roomIds.Length > 0)
            {
                int deletedCount = _roomRepository.DeleteWithIds(roomIds, "RoomId");
                result.DeleteCount = deletedCount;
                result.Message = $"{deletedCount} row(s) deleted successfully.";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "No selected for deletion.";
            }
            return result;
        }
        public Results<ListScreenModel<RoomSearchModel>> GetList(PageModel pageModel)
        {
            var result = new Results<ListScreenModel<RoomSearchModel>>();
            result.IsSuccess = true;

            var search = !string.IsNullOrEmpty(pageModel.Search) 
                ? JsonConvert.DeserializeObject<RoomSearchModel>(pageModel.Search) 
                : new RoomSearchModel();

            Expression<Func<Room, bool>> filter = x =>
                (
                    (string.IsNullOrEmpty(search.RoomName) || x.RoomName.Contains(search.RoomName))
                    && (string.IsNullOrEmpty(search.RoomDescription) || x.RoomDescription.Contains(search.RoomDescription))
                    && (string.IsNullOrEmpty(search.RoomCode) || x.RoomCode.Contains(search.RoomCode))
                );

            var listScreen = new ListScreenModel<RoomSearchModel>()
            {
                Data = _roomRepository.GetAllWithOptionsAndIncludes(pageModel, filter).Cast<object>().ToList(),
                Page = 1,
                PageSize = pageModel.PageSize,
                DataCount = _roomRepository.GetCountWithOptions(pageModel, filter),
                PageMode = pageModel,
                SearchModel = search
            };
            result.Data = listScreen;
            return result;
        }
    }
}
