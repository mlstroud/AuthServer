using System.Threading.Tasks;
using AuthServer.Library.Enums;
using AuthServer.Library.Models;
using AuthServer.Library.DataAccess;
using AuthServer.Library.Repositories;
using AuthServer.Library.Models.Requests;
using Serilog;

namespace AuthServer.Library.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptographyService _cryptographyService;

        public UserService(IUserRepository userRepository, ICryptographyService cryptographyService)
        {
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
        }

        public async Task<bool> RegisterUser(RegisterRequest request)
        {
            var salt = _cryptographyService.GetSalt();

            var user = new User
            {
                Username = request.Username,
                Password =  _cryptographyService.GetHash(request.Password, salt),
                Salt = salt,
                UserType = UserType.Member
            };

            var result = await _userRepository.InsertUser(user);

            switch (result.Status)
            {
                case DbResponseStatus.Success:
                    return true;
                case DbResponseStatus.NoRowsAffected:
                    Log.Error($"Unable to register user [{request.Username}] because it already exists.");
                    return false;
                case DbResponseStatus.Failure:
                    return false;
                default:
                    Log.Error($"There was an unexpected error trying to register user [{user.Username}].");
                    return false;
            }
        }

        public async Task<int> GetUserId(string username)
        {
            var result = await _userRepository.GetUserId(username);

            switch (result.Status)
            {
                case DbResponseStatus.Success:
                    return result.Value;
                case DbResponseStatus.NotFound:
                    Log.Error($"Unable to find id for user [{username}].");
                    return 0;
                default:
                    Log.Error($"There was an unexpected error finding the id for user [{username}].");
                    return 0;
            }
        }

        public async Task<User> Authenticate(LoginRequest request)
        {
            var user = await _userRepository.GetUser(request.Username);

            if (user.Status != DbResponseStatus.Success)
            {
                Log.Error($"Unable to find user [{request.Username}].");
                return null;
            }

            var hashedPassword = _cryptographyService.GetHash(request.Password, user.Value.Salt);

            if (hashedPassword != user.Value.Password)
            {
                Log.Information($"Login attempt failed for user [{user.Value.Username}] due to incorrect password.");
                return null;
            }

            return user.Value;
        }
    }
}
