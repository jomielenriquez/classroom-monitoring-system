using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
namespace classroom_monitoring_system.Repository
{
    public class UserRepository
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        public UserRepository(IBaseRepository<User> baseRepository, IBaseRepository<UserRole> userRoleRepository)
        {
            _userRepository = baseRepository;
            _userRoleRepository = userRoleRepository;
        }

        public Results<User> GetUserById(Guid userId)
        {
            var result = new Results<User>();
            result.IsSuccess = true;
            result.Data = _userRepository.GetByCondition(
                x => x.UserId == userId,
                x => x.UserRole).FirstOrDefault() ?? new User()
                {
                    CreatedDate = DateTime.Now
                };
            return result;
        }
        private Results<List<string>> IsValid(User user)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(user.FirstName))
            {
                errors.Add("First Name is required.");
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                errors.Add("Last Name is required.");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.Add("Email is required.");
            }

            if (user.UserId == Guid.Empty)
            {
                var existingUser = _userRepository.GetByCondition(x =>
                    x.Email == user.Email);

                if (existingUser.Any() && existingUser.Where(x => x.Email == user.Email).Any())
                {
                    errors.Add("Email already exists.");
                }
            }
            return new Results<List<string>> { IsSuccess = !errors.Any(), Errors = errors };
        }
        public Results<User> SaveUser(User user)
        {
            var result = new Results<User>();
            result.IsSuccess = true;
            result.Data = user;
            result.Errors = new List<string>();

            var validationResult = IsValid(user);
            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Errors = validationResult.Errors;
                return result;
            }

            if (user.UserId == Guid.Empty)
            {
                user.Password = LoginRepository.ComputeMd5Hash(user.Password);
                result.Data = _userRepository.Save(user);
            }
            else
            {
                var userResult = _userRepository.GetById(user.UserId);
                userResult.FirstName = user.FirstName;
                userResult.LastName = user.LastName;
                userResult.Email = user.Email;
                userResult.UserName = user.UserName;
                userResult.Password = !string.IsNullOrEmpty(user.Password)
                    ? LoginRepository.ComputeMd5Hash(user.Password)
                    : userResult.Password;
                userResult.UserRoleId = user.UserRoleId;
                result.Data = _userRepository.Update(userResult);
            }

            return result;
        }
        public Results<User> DeleteUser(Guid[] userIds)
        {
            var result = new Results<User>();
            result.IsSuccess = true;
            result.DeleteCount = 0;
            if (userIds != null && userIds.Length > 0)
            {
                foreach(var userId in userIds)
                {
                    var user = _userRepository.GetByConditionAndIncludes(x => x.UserId == userId, "UserRole").FirstOrDefault();
                    if(user.UserRole.RoleName.ToLower() == "admin")
                    {
                        result.IsSuccess = false;
                        result.Message = "Admin users cannot be deleted.";
                        return result;
                    }
                }
                int deletedCount = _userRepository.DeleteWithIds(userIds, "UserId");
                result.DeleteCount = deletedCount;
                result.Message = $"{deletedCount} user(s) deleted successfully.";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "No users selected for deletion.";
            }
            return result;
        }
        public Results<ListScreenModel<UserSearchModel>> GetList(PageModel pageModel)
        {
            var result = new Results<ListScreenModel<UserSearchModel>>();
            result.IsSuccess = true;

            var search = !string.IsNullOrEmpty(pageModel.Search) ? JsonConvert.DeserializeObject<UserSearchModel>(pageModel.Search) : new UserSearchModel();

            Expression<Func<User, bool>> filter = x =>
                (
                    (string.IsNullOrEmpty(search.FirstName) || x.FirstName.Contains(search.FirstName))
                    && (string.IsNullOrEmpty(search.LastName) || x.LastName.Contains(search.LastName))
                );

            var listScreen = new ListScreenModel<UserSearchModel>()
            {
                Data = _userRepository.GetAllWithOptionsAndIncludes(pageModel, filter, "UserRole").Cast<object>().ToList(),
                Page = 1,
                PageSize = pageModel.PageSize,
                DataCount = _userRepository.GetCountWithOptions(pageModel, filter),
                PageMode = pageModel,
                SearchModel = search
            };
            result.Data = listScreen;
            return result;
        }
    }
}
