using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Serilog;

namespace AuthServer.Library.DataAccess
{
    public static class SprocExtensions
    {
        public static SprocNonQuery Initialize(this SprocNonQuery procedure, string procedureName, DynamicParameters parameters) =>
            new SprocNonQuery
            {
                ProcedureName = procedureName,
                Paramters = parameters
            };

        public static SprocQuerySingle<T> Initialize<T>(this SprocQuerySingle<T> procedure, string procedureName, DynamicParameters parameters) =>
            new SprocQuerySingle<T>
            {
                ProcedureName = procedureName,
                Paramters = parameters
            };

        public static SprocQueryMany<T> Initialize<T>(this SprocQueryMany<T> procedure, string procedureName, DynamicParameters parameters) =>
            new SprocQueryMany<T>
            {
                ProcedureName = procedureName,
                Paramters = parameters
            };

        public static async Task<DbResponse> Execute(this SprocNonQuery procedure)
        {
            try
            {
                using var conn = DbHelper.CreateConnection();

                var result = await conn.ExecuteAsync(
                    procedure.ProcedureName,
                    procedure.Paramters,
                    commandType: CommandType.StoredProcedure);

                if (result == 0)
                    return DbResponse.Failure(DbResponseStatus.NoRowsAffected);

                return DbResponse.Success();
            }
            catch (SqlException e)
            {
                Log.Error(e, $"Theree was an unexpected error executing the procedure [{procedure.ProcedureName}]");
                return DbResponse.Failure(DbResponseStatus.Failure);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Theree was an unexpected error executing the procedure [{procedure.ProcedureName}]");
                return DbResponse.Failure(DbResponseStatus.Failure);
            }
        }

        public static async Task<DbResponse<T>> Execute<T>(this SprocQuerySingle<T> procedure)
        {
            try
            {
                using var conn = DbHelper.CreateConnection();

                var result = await conn.QuerySingleOrDefaultAsync<T>(
                    procedure.ProcedureName,
                    procedure.Paramters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                    return DbResponse<T>.NotFound<T>(); ;

                return DbResponse<T>.Success<T>(result);
            }
            catch (SqlException e)
            {
                Log.Error(e, $"Theree was an unexpected error executing the procedure [{procedure.ProcedureName}]");
                return DbResponse<T>.Failure<T>(DbResponseStatus.Failure);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Theree was an unexpected error executing the procedure [{procedure.ProcedureName}]");
                return DbResponse<T>.Failure<T>(DbResponseStatus.Failure);
            }
        }

        public static async Task<DbResponse<T>> Execute<T>(this SprocQueryMany<T> procedure)
        {
            try
            {
                using var conn = DbHelper.CreateConnection();

                var result = await conn.QueryAsync<T>(
                    procedure.ProcedureName,
                    procedure.Paramters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                    return DbResponse<T>.NotFound<T>(); ;

                return DbResponse<T>.Success<T>(result);
            }
            catch (SqlException e)
            {
                Log.Error(e, $"Theree was an unexpected error executing the procedure [{procedure.ProcedureName}]");
                return DbResponse<T>.Failure<T>(DbResponseStatus.Failure);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Theree was an unexpected error executing the procedure [{procedure.ProcedureName}]");
                return DbResponse<T>.Failure<T>(DbResponseStatus.Failure);
            }
        }
    }
}
