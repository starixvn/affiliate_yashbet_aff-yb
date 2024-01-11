using Fanex.Affiliate.User.Core.Infrastructure;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System;
using System.Security.Cryptography;
using System.Web;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Infrastructure
{
    public class HttpRequestParserHandlerTest
    {
        private readonly HttpRequestBase Request;
        private readonly IAppSetting AppSetting;
        private readonly IDateTimeProvider DateTimeProvider;

        private static readonly long RequestTickCount = (new DateTime(2020, 10, 10)).Ticks;
        private const long ExpriredMin = 10;

        private const string PASSWORD = "password";
        private const string SALT = "ABCDEFGHABCDEF";

        public HttpRequestParserHandlerTest()
        {
            Request = Substitute.For<HttpRequestBase>();
            AppSetting = Substitute.For<IAppSetting>();
            DateTimeProvider = Substitute.For<IDateTimeProvider>();
        }

        [Fact]
        public void TryParseQueryString_WhenEncryptedStringIsEmpty_ReturnNotVaildQueryString()
        {
            var parser = CreateParser();

            var result = parser.TryParseQueryString(string.Empty);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void TryParseQueryString_WhenIsExpiredRequest_ReturnExpiredQueryString()
        {
            var parser = CreateParser();
            var queryString = GenQueryString();
            var encodedQueryString = SymmetricEncryptionUtility.Encrypt<AesManaged>(queryString, PASSWORD, SALT);
            var base64String = ConverterUtils.Base64Encode(encodedQueryString);
            Request.Params["t"] = RequestTickCount.ToString();
            DateTimeProvider.Now.Returns(new DateTime(RequestTickCount).AddMinutes(ExpriredMin + 1));

            var result = parser.TryParseQueryString(base64String);

            Assert.False(result.IsValid);
            Assert.True(result.IsExpired);
        }

        [Fact]
        public void TryParseQueryString_WhenCanNotParseControllerName_ReturnNotValidQueryString()
        {
            var parser = CreateParser();
            var queryString = GenQueryString();
            var encodedQueryString = SymmetricEncryptionUtility.Encrypt<AesManaged>(queryString, PASSWORD, SALT);
            var base64String = ConverterUtils.Base64Encode(encodedQueryString);
            Request.Params["t"] = RequestTickCount.ToString();
            DateTimeProvider.Now.Returns(new DateTime(RequestTickCount));

            var result = parser.TryParseQueryString(base64String);

            Assert.False(result.IsValid);
            Assert.Null(result.ActionName);
            Assert.Null(result.ControllerName);
        }

        [Fact]
        public void TryParseQueryString_WhenCallWithValidQueryString_ReturnIsValidResult()
        {
            var parser = CreateParser();
            var action = "action";
            var controller = "controller";
            var queryString = $"{GenQueryString()}&action={action}&controller={controller}";
            var encodedQueryString = SymmetricEncryptionUtility.Encrypt<AesManaged>(queryString, PASSWORD, SALT);
            var base64String = ConverterUtils.Base64Encode(encodedQueryString);
            Request.Params["t"] = RequestTickCount.ToString();
            DateTimeProvider.Now.Returns(new DateTime(RequestTickCount));

            var result = parser.TryParseQueryString(base64String);

            Assert.True(result.IsValid);
            Assert.Equal(action, result.ActionName);
            Assert.Equal(controller, result.ControllerName);
        }

        [Fact]
        public void TryParseQueryString_WhenCallWithNullParam_ReturnIsValidResult()
        {
            var parser = CreateParser();
            var action = "action";
            var controller = "controller";
            var queryString = $"action={action}&controller={controller}";
            var encodedQueryString = SymmetricEncryptionUtility.Encrypt<AesManaged>(queryString, PASSWORD, SALT);
            var base64String = ConverterUtils.Base64Encode(encodedQueryString);

            var result = parser.TryParseQueryString(base64String);

            Assert.True(result.IsValid);
            Assert.Equal(action, result.ActionName);
            Assert.Equal(controller, result.ControllerName);
        }

        private static string GenQueryString()
            => $"t={RequestTickCount}&e={ExpriredMin}";

        private HttpRequestParserHandler CreateParser()
        {
            var parser = new HttpRequestParserHandler(Request)
            {
                AppSetting = AppSetting,
                DateTimeProvider = DateTimeProvider
            };
            AppSetting.SymmetricPassword.Returns(PASSWORD);
            AppSetting.SymmetricSalt.Returns(SALT);
            Request.Params.Returns(new System.Collections.Specialized.NameValueCollection());
            return parser;
        }
    }
}
