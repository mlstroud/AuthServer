using System.Data;
using System.Data.SqlClient;
using AuthServer.Library.Config;
using Serilog;

namespace AuthServer.Library.DataAccess
{
    public static class DbHelper
    {
        private static IDbConfig _config;
        public static void SetConfig(IDbConfig config) =>
            _config = config;

        public static IDbConnection CreateConnection() =>
            new SqlConnection(_config.ConnectionString);
    }
}
