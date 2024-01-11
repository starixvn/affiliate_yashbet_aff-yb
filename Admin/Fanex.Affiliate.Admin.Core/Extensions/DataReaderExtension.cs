using System;
using System.Data;
using System.Data.SqlTypes;

namespace Fanex.Affiliate.Admin.Core.Extensions
{
    public static class DataReaderExtension
    {
        public static bool HasColumn(this IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

#pragma warning disable IDE0034 // Simplify 'default' expression
        public static T GetVal<T>(this IDataReader dataReader, string columnName, T defauleValue = default(T))
        {
            var dbValue = dataReader[columnName];
            if (dbValue == DBNull.Value)
            {
                return defauleValue;
            }

            return Cast<T>(dbValue, defauleValue);
        }

        private static T Cast<T>(object dbValue, T defauleValue = default(T))
        {
            if (dbValue is T t)
            {
                return t;
            }

            if (dbValue is INullable && (dbValue as INullable).IsNull)
            {
                return defauleValue;
            }

            return (T)Convert.ChangeType(dbValue, typeof(T));
        }
    }
}