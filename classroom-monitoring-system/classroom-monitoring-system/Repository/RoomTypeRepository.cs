using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
namespace classroom_monitoring_system.Repository
{
    public class RoomTypeRepository
    {
        private readonly IBaseRepository<RoomType> _roomTypeRepository;
        public RoomTypeRepository(IBaseRepository<RoomType> roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public Results<RoomType> GetRoomTypeById(Guid roomTypeId)
        {
            var result = new Results<RoomType>();
            result.IsSuccess = true;
            result.Data = _roomTypeRepository.GetByCondition(
                x => x.RoomTypeId == roomTypeId).FirstOrDefault() ?? new RoomType()
                {
                    CreatedDate = DateTime.Now
                };
            return result;
        }

        private Results<List<string>> IsValid(RoomType user)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(user.RoomTypeName))
            {
                errors.Add("Room Name is required.");
            }
            if (string.IsNullOrEmpty(user.RoomTypeDescription))
            {
                errors.Add("Room description is required.");
            }
            return new Results<List<string>> { IsSuccess = !errors.Any(), Errors = errors };
        }

        public Results<RoomType> SaveRoomType(RoomType roomType)
        {
            var result = new Results<RoomType>();
            result.IsSuccess = true;
            result.Data = roomType;
            result.Errors = new List<string>();

            var validationResult = IsValid(roomType);
            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Errors = validationResult.Errors;
                return result;
            }

            if (roomType.RoomTypeId == Guid.Empty)
            {
                roomType.CreatedDate = DateTime.Now;
                result.Data = _roomTypeRepository.Save(roomType);
            }
            else
            {
                var dataResult = _roomTypeRepository.GetById(roomType.RoomTypeId);
                dataResult.RoomTypeName = roomType.RoomTypeName;
                dataResult.RoomTypeDescription = roomType.RoomTypeDescription;
                result.Data = _roomTypeRepository.Update(dataResult);
            }

            return result;
        }
        public Results<RoomType> DeleteRoomType(Guid[] roomTypeIds)
        {
            var result = new Results<RoomType>();
            result.IsSuccess = true;
            result.DeleteCount = 0;
            if (roomTypeIds != null && roomTypeIds.Length > 0)
            {
                int deletedCount = _roomTypeRepository.DeleteWithIds(roomTypeIds, "RoomTypeId");
                result.DeleteCount = deletedCount;
                result.Message = $"{deletedCount} room type(s) deleted successfully.";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "No selected for deletion.";
            }
            return result;
        }
        public Results<ListScreenModel<RoomTypeSearchModel>> GetList(PageModel pageModel)
        {
            var result = new Results<ListScreenModel<RoomTypeSearchModel>>();
            result.IsSuccess = true;

            var search = !string.IsNullOrEmpty(pageModel.Search) ? JsonConvert.DeserializeObject<RoomTypeSearchModel>(pageModel.Search) : new RoomTypeSearchModel();

            Expression<Func<RoomType, bool>> filter = x =>
                (
                    (string.IsNullOrEmpty(search.RoomTypeName) || x.RoomTypeName.Contains(search.RoomTypeName))
                );

            var listScreen = new ListScreenModel<RoomTypeSearchModel>()
            {
                Data = _roomTypeRepository.GetAllWithOptionsAndIncludes(pageModel, filter).Cast<object>().ToList(),
                Page = 1,
                PageSize = pageModel.PageSize,
                DataCount = _roomTypeRepository.GetCountWithOptions(pageModel, filter),
                PageMode = pageModel,
                SearchModel = search
            };
            result.Data = listScreen;
            return result;
        }
    }
}
