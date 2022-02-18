using System.Threading.Tasks;
using AuthServer.Library.Models;
using AuthServer.Library.DataAccess;

namespace AuthServer.Library.Repositories
{
    public interface IUserRepository
    {
        Task<DbResponse> InsertUser(User user);
        Task<DbResponse<int>> GetUserId(string username);
        Task<DbResponse<User>> GetUser(string username);
    }
}
