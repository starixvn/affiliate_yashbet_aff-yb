using System;
using System.Data;

namespace Affiliate.YB.WebApi.Extensions
{
    public static class ObjectDbExtensions
    {
        public static T GetVal<T>(this IDataReader reader, string rowName)
        {
            if(IsNullable(typeof(T)))
            {
                return reader[rowName] == DBNull.Value ? default(T) : (T)Convert.ChangeType(reader[rowName], Nullable.GetUnderlyingType(typeof(T)));
            }
            return reader[rowName] == DBNull.Value ? default(T) : (T)Convert.ChangeType(reader[rowName], typeof(T));
        }

        private static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;
    }
}