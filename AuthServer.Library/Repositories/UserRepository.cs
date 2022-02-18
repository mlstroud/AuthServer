using System.Data;
using System.Threading.Tasks;
using AuthServer.Library.Models;
using AuthServer.Library.DataAccess;
using Dapper;

namespace AuthServer.Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string InsertUserProcedureName = "dbo.Insert_User";
        private const string GetUserIdProcedureName = "dbo.Get_UserId";
        private const string GetUserProcedureName = "dbo.Get_User";

        private SprocNonQuery _insertUserProcedure;
        private SprocQuerySingle<int> _getUserIdProcedure;
        private SprocQuerySingle<User> _getUserProcedure;

        public async Task<DbResponse> InsertUser(User user) =>
            await _insertUserProcedure
                .Initialize(InsertUserProcedureName, MapInsertUserParameters(user))
                .Execute();

        public async Task<DbResponse<int>> GetUserId(string username) =>
            await _getUserIdProcedure
                .Initialize(GetUserIdProcedureName, MapGetUserIdParameters(username))
                .Execute();

        public async Task<DbResponse<User>> GetUser(string username) =>
            await _getUserProcedure
                .Initialize(GetUserProcedureName, MapGetUserParameters(username))
                .Execute();

        private DynamicParameters MapInsertUserParameters(User user)
        {
            var p = new DynamicParameters();

            p.Add(InputParameters.Username, user.Username, DbType.String);
            p.Add(InputParameters.Password, user.Password, DbType.String);
            p.Add(InputParameters.Salt, user.Salt, DbType.String);
            p.Add(InputParameters.UserType, user.UserType, DbType.Int32);

            return p;
        }

        private DynamicParameters MapGetUserIdParameters(string username)
        {
            var p = new DynamicParameters();

            p.Add(InputParameters.Username, username, DbType.String);

            return p;
        }

        private DynamicParameters MapGetUserParameters(string username)
        {
            var p = new DynamicParameters();

            p.Add(InputParameters.Username, username, DbType.String);

            return p;
        }
    }
}
