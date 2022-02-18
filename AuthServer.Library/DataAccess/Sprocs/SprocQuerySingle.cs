using Dapper;

namespace AuthServer.Library.DataAccess
{
    public class SprocQuerySingle<T>
    {
        public string ProcedureName { get; set; }
        public DynamicParameters Paramters { get; set; }
    }
}
