using Fanex.Affiliate.Link.Extensions;
using Fanex.Data;
using Fanex.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Fanex.Affiliate.Link.Helpers
{
    public static class LogHelper
    {
        private const int LimitedLevelOfInnerException = 3;

        public static void WriteError(Exception exception, string additionalMessage = null)
        {
            var content = BuildMessage(exception);
            if (!string.IsNullOrWhiteSpace(additionalMessage))
            {
                content.AppendLine(additionalMessage);
            }

            AddDatabaseInfo(content, exception);
            AddExceptionInfo(content, exception, LimitedLevelOfInnerException);

            Logger.Log.Error(content.ToString(), exception);
        }

        private static StringBuilder BuildMessage(Exception exception)
        {
            var request = HttpContext.Current.Request;
            var builder = new StringBuilder().AppendLine($"Error Message: {exception.Message}").AppendLine();
            builder.AppendLine("REQUEST INFO")
                   .AppendLine("Domain: " + request.Headers["Host"])
                   .AppendLine("Form: " + string.Join("~", request.Form))
                   .AppendLine("Url: " + request.RawUrl)
                   .AppendLine("Referrer: " + request.UrlReferrer)
                   .AppendLine("Query String: " + string.Join("~", request.QueryString))
                   .AppendLine("Browser: " + request.Browser.Type)
                   .AppendLine("BrowserVer: " + request.Browser.Version)
                   .AppendLine("Agent: " + request.UserAgent)
                   .AppendLine("IP Address: " + request.UserHostAddress)
                   .AppendLine("Actual IP Address: " + request.GetClientIP())
                   .AppendLine($"Source:{exception.Source}")
                   .AppendLine($"TargetSite:{exception.TargetSite}");

            return builder;
        }

        private static void AddDatabaseInfo(StringBuilder builder, Exception exception)
        {
            builder.AppendLine();
            builder.AppendLine("DATABASE INFO");

            var dbInfo = GetDbExceptionInfo(exception);

            foreach (var item in dbInfo)
            {
                builder.AppendLine($"{item.Key}: {item.Value}");
            }
        }

        private static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
        {
            var dbInfo = new Dictionary<string, object>();
            BindSqlExceptionInfo(exception, dbInfo);
            BindFanexDataExceptionInfo(exception, dbInfo);
            return dbInfo;
        }

        private static void BindFanexDataExceptionInfo(Exception exception, IDictionary<string, object> dbInfo)
        {
            if (exception is ObjectDbException fanexDataException)
            {
                BindSqlExceptionInfo(exception.InnerException, dbInfo);

                var connString = new SqlConnectionStringBuilder(fanexDataException.ConnectionString);

                dbInfo["Server"] = connString.DataSource;
                dbInfo["DbName"] = fanexDataException.DbName;
                dbInfo["SpName"] = fanexDataException.SpName;
                dbInfo["UserId"] = connString.UserID;
                dbInfo["Parameters"] = GetParameters(fanexDataException.Parameters);
                dbInfo["CommandTimeout"] = fanexDataException.CommandTimeout;
            }
        }

        private static void BindSqlExceptionInfo(Exception exception, IDictionary<string, object> dbInfo)
        {
            if (exception is SqlException sqlException)
            {
                dbInfo["Server"] = sqlException.Server;
                dbInfo["Procedure"] = sqlException.Procedure;
                dbInfo["Line"] = sqlException.LineNumber;
            }
        }

        private static string GetParameters(IDictionary<string, object> parameters)
            => parameters?.Count > 0
               ? parameters.Select(parameter => $"{parameter.Key}={parameter.Value}")
                        .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
               : string.Empty;

        private static void AddExceptionInfo(StringBuilder builder, Exception exception, int limitedInnerException)
        {
            builder.AppendLine();
            builder.AppendLine($"EXCEPTION INFO LEVEL: {LimitedLevelOfInnerException - limitedInnerException}");
            builder.AppendLine($"Message: {exception.Message}");
            builder.AppendLine($"Source: {exception.Source}");
            builder.AppendLine($"TargetSite: {exception.TargetSite}");
            builder.AppendLine($"StackTrace: {exception.StackTrace}");

            if (exception.InnerException != null && limitedInnerException > 0)
            {
                AddExceptionInfo(builder, exception.InnerException, --limitedInnerException);
            }
        }
    }
}
