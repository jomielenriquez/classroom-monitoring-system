using classroom_monitoring_system.Models;
using classroom_monitoring_system.Interface;
using System.Security.Cryptography;
using System.Text;

namespace classroom_monitoring_system.Repository
{
    public class LoginRepository
    {
        private readonly IBaseRepository<User> _baseRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        public LoginRepository(IBaseRepository<User> baseRepository, IBaseRepository<UserRole> userRoleRepository)
        {
            _baseRepository = baseRepository;
            _userRoleRepository = userRoleRepository;
        }
        public static string ComputeMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
