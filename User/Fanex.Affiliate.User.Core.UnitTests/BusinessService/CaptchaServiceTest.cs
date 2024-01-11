using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class CaptchaServiceTest
    {
        private readonly IAppSetting AppSetting;

        public CaptchaServiceTest()
        {
            AppSetting = Substitute.For<IAppSetting>();
        }

        [Fact]
        public void GenerateCaptcha_WhenCall_ReturnNotEmptyString()
        {
            var service = CreateService();

            var captcha = service.GenerateCaptcha();

            Assert.NotEmpty(captcha);
        }

        [Fact]
        public void IsValid_WhenCaptchaIsSameAsSessionCaptcha_ReturnTrue()
        {
            var service = CreateService();

            var result = service.IsValid("abcd", "abcd", "127.0.0.1");

            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenUseSecretCaptchaInLocal_ReturnTrue()
        {
            var service = CreateService();
            var secretCaptcha = "1111";
            AppSetting.GetValue("AllowAutomationCaptchaCode").Returns("1");
            AppSetting.GetValue("SecretCaptcha").Returns(secretCaptcha);
            AppSetting.IsLocal.Returns(true);

            var result = service.IsValid(secretCaptcha, "", "127.0.0.1");

            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenUseSecretCaptchaOnPermitedIpAddress_ReturnTrue()
        {
            var service = CreateService();
            var secretCaptcha = "1111";
            var ipAddress = "202.73.231.33";
            AppSetting.GetValue("AllowAutomationCaptchaCode").Returns("1");
            AppSetting.GetValue("SecretCaptcha").Returns(secretCaptcha);
            AppSetting.IsLocal.Returns(false);
            AppSetting.GetValue("CompanyIp").Returns("^(202\\.78\\.231\\.(3[3-9]|[4-5][0-9]|6[0-2])|125\\.227\\.140\\.97)$");

            var result = service.IsValid(secretCaptcha, "", ipAddress);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenUseWrongSecretCaptcha_ReturnFalse()
        {
            var service = CreateService();
            var secretCaptcha = "1111";
            var ipAddress = "202.73.231.33";
            AppSetting.GetValue("AllowAutomationCaptchaCode").Returns("1");
            AppSetting.GetValue("SecretCaptcha").Returns(secretCaptcha);
            AppSetting.IsLocal.Returns(false);
            AppSetting.GetValue("CompanyIp").Returns("^(202\\.78\\.231\\.(3[3-9]|[4-5][0-9]|6[0-2])|125\\.227\\.140\\.97)$");

            var result = service.IsValid("21", "", ipAddress);

            Assert.False(result);
        }

        [Fact]
        public void GenerateImageBuffer_WhenNeedDrawComplexImage_ReturnNotNull()
        {
            var service = CreateService();

            var buffer = service.GenerateImageBuffer("1234", 120, 30, true);

            Assert.NotNull(buffer);
        }

        [Fact]
        public void GenerateImageBuffer_WhenCall_ReturnNotNull()
        {
            var service = CreateService();

            var buffer = service.GenerateImageBuffer("1234", 120, 30, false);

            Assert.NotNull(buffer);
        }

        public CaptchaService CreateService()
            => new CaptchaService(AppSetting);
    }
}
