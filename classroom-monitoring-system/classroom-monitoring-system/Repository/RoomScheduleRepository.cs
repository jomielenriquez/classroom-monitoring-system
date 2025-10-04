using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
namespace classroom_monitoring_system.Repository
{
    public class RoomScheduleRepository
    {
        private readonly IBaseRepository<RoomSchedule> _roomScheduleRepository;
        public RoomScheduleRepository(IBaseRepository<RoomSchedule> roomScheduleRepository)
        {
            _roomScheduleRepository = roomScheduleRepository;
        }
        public Results<RoomSchedule> GetRoomById(Guid roomScheduleId)
        {
            var result = new Results<RoomSchedule>();
            result.IsSuccess = true;
            result.Data = _roomScheduleRepository.GetByCondition(
                x => x.RoomScheduleId == roomScheduleId).FirstOrDefault() ?? new RoomSchedule()
                {
                    CreatedDate = DateTime.Now,
                    DateOfUse = DateOnly.FromDateTime(DateTime.Now)
                };
            return result;
        }
        private Results<List<string>> IsValid(RoomSchedule roomSchedule)
        {
            var errors = new List<string>();
            if (roomSchedule.ProfessorUserId == Guid.Empty)
            {
                errors.Add("Room Name is required.");
            }
            if (roomSchedule.RoomId == Guid.Empty)
            {
                errors.Add("Room is required.");
            }
            //if (roomSchedule.DateOfUse > DateOnly.FromDateTime(DateTime.Now))
            //{
            //    errors.Add("Room is required.");
            //}
            return new Results<List<string>> { IsSuccess = !errors.Any(), Errors = errors };
        }
        public Results<RoomSchedule> SaveRoomSchedule(RoomSchedule roomSchedule)
        {
            var result = new Results<RoomSchedule>();
            result.IsSuccess = true;
            result.Data = roomSchedule;
            result.Errors = new List<string>();
            var validationResult = IsValid(roomSchedule);
            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Errors = validationResult.Errors;
                return result;
            }
            if (roomSchedule.RoomScheduleId == Guid.Empty)
            {
                result.Data = _roomScheduleRepository.Save(roomSchedule);
            }
            else
            {
                result.Data = _roomScheduleRepository.Update(roomSchedule);
            }
            return result;
        }
        public Results<RoomSchedule> DeleteRoomSchedule(Guid[] roomScheduleIds)
        {
            var result = new Results<RoomSchedule>();
            result.IsSuccess = true;
            result.DeleteCount = 0;
            if (roomScheduleIds != null && roomScheduleIds.Length > 0)
            {
                int deletedCount = _roomScheduleRepository.DeleteWithIds(roomScheduleIds, "RoomScheduleId");
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
        public Results<ListScreenModel<RoomScheduleSearchModel>> GetList(PageModel pageModel)
        {
            var result = new Results<ListScreenModel<RoomScheduleSearchModel>>();
            result.IsSuccess = true;

            var search = !string.IsNullOrEmpty(pageModel.Search)
                ? JsonConvert.DeserializeObject<RoomScheduleSearchModel>(pageModel.Search)
                : new RoomScheduleSearchModel();

            Expression<Func<RoomSchedule, bool>> filter = x =>
                (
                    (string.IsNullOrEmpty(search.RoomCode) || x.Room.RoomCode.Contains(search.RoomCode))
                );

            var listScreen = new ListScreenModel<RoomScheduleSearchModel>()
            {
                Data = _roomScheduleRepository.GetAllWithOptionsAndIncludes(pageModel, filter, "ProfessorUser", "Room").Cast<object>().ToList(),
                Page = 1,
                PageSize = pageModel.PageSize,
                DataCount = _roomScheduleRepository.GetCountWithOptions(pageModel, filter),
                PageMode = pageModel,
                SearchModel = search
            };
            result.Data = listScreen;
            return result;
        }
    }
}
