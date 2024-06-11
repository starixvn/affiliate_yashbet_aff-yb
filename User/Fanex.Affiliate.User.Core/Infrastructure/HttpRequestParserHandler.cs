using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Infrastructure
{
    public class HttpRequestParserHandler : IHttpRequestParserHandler
    {
        private readonly HttpRequestBase _request;

        public HttpRequestParserHandler(HttpRequestBase request)
        {
            _request = request;
        }

        public IAppSetting AppSetting { get; set; }

        public IDateTimeProvider DateTimeProvider { get; set; }

        public QueryStringResult TryParseQueryString(string encryptedQuery)
        {
            if (string.IsNullOrWhiteSpace(encryptedQuery))
            {
                return new QueryStringResult { IsValid = false };
            }

            var queryString = DecryptQueryString(encryptedQuery);
            var paramValues = HttpUtility.ParseQueryString(queryString);

            if (CheckIsExpiredRequest(paramValues))
            {
                return new QueryStringResult { IsValid = false, IsExpired = true };
            }

            var controllerName = paramValues["controller"]?.Trim();
            var actionName = paramValues["action"]?.Trim();

            return new QueryStringResult
            {
                IsValid = !string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName),
                ControllerName = controllerName,
                ActionName = actionName
            };
        }

        protected virtual string DecryptQueryString(string encryptedQuery)
        {
            var appSetting = AppSetting ?? DependencyResolver.Current.GetService<IAppSetting>();

            var decodedString = ConverterUtils.Base64Decode(encryptedQuery);

            return SymmetricEncryptionUtility.Decrypt<AesManaged>(decodedString, appSetting.SymmetricPassword, appSetting.SymmetricSalt);
        }

        private bool CheckIsExpiredRequest(NameValueCollection paramValues)
        {
            var timeQuery = _request.Params["t"]?.Trim();
            var timeToken = paramValues["t"]?.Trim();
            var expirationByMinute = int.Parse(paramValues["e"]?.Trim() ?? "0");

            return !string.Equals(timeQuery, timeToken) || IsExpired(timeToken, expirationByMinute);
        }

        private bool IsExpired(string timeTickString, int expirationByMinute)
        {
            var dateTimeProvider = DateTimeProvider ?? DependencyResolver.Current.GetService<IDateTimeProvider>();
            var minDateTime = dateTimeProvider.Now.AddMinutes(expirationByMinute * -1);
            var timeTicks = Convert.ToInt64(timeTickString);
            var dateTimeRequest = new DateTimeOffset(timeTicks, TimeSpan.Zero).DateTime;

            return DateTime.Compare(dateTimeRequest, minDateTime) < 0;
        }
    }
}