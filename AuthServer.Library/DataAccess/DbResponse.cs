using System.Collections.Generic;

namespace AuthServer.Library.DataAccess
{
    public class DbResponse
    {
        public DbResponseStatus Status { get; set; }

        public DbResponse(DbResponseStatus status) => Status = status;

        public static DbResponse Success() =>
            new DbResponse(DbResponseStatus.Success);

        public static DbResponse Failure(DbResponseStatus status) =>
            new DbResponse(status);
    }

    public class DbResponse<T>
    {
        public DbResponseStatus Status { get; set; }
        public T Value { get; set; }
        public IEnumerable<T> Values { get; set; }

        public DbResponse(DbResponseStatus status)
        {
            Value = default;
            Values = null;
            Status = status;
        }

        public DbResponse(T value, DbResponseStatus status)
        {
            Value = value;
            Values = null;
            Status = status;
        }

        public DbResponse(IEnumerable<T> values, DbResponseStatus status)
        {
            Value = default;
            Values = values;
            Status = status;
        }

        public static DbResponse<T> Success<T>(T value) =>
            new DbResponse<T>(value, DbResponseStatus.Success);

        public static DbResponse<T> Success<T>(IEnumerable<T> values) =>
            new DbResponse<T>(values, DbResponseStatus.Success);

        public static DbResponse<T> NotFound<T>() =>
            new DbResponse<T>(DbResponseStatus.NotFound);

        public static DbResponse<T> Failure<T>(DbResponseStatus status) =>
            new DbResponse<T>(status);
    }
}
