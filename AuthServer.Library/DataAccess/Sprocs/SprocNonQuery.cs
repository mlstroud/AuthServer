using Dapper;

namespace AuthServer.Library.DataAccess
{
    public class SprocNonQuery
    {
        public string ProcedureName { get; set; }
        public DynamicParameters Paramters { get; set; }
    }
}
