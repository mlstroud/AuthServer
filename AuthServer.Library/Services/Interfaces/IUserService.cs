using System.Threading.Tasks;
using AuthServer.Library.Models;
using AuthServer.Library.Models.Requests;

namespace AuthServer.Library.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterRequest request);
        Task<int> GetUserId(string username);
        Task<User> Authenticate(LoginRequest request);
    }
}
