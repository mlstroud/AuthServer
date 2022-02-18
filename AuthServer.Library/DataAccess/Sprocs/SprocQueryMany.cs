using Dapper;

namespace AuthServer.Library.DataAccess
{
    public class SprocQueryMany<T>
    {
        public string ProcedureName { get; set; }
        public DynamicParameters Paramters { get; set; }
    }
}
