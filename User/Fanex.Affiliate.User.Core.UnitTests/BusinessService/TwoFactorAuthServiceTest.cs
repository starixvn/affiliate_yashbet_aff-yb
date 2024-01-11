using Fanex.Affiliate.User.Core.BusinessService;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class TwoFactorAuthServiceTest
    {
        [Fact]
        public void VerifyHotp_WhenCallWithCorrectOtp_ReturnTrue()
        {
            var secretKey = "abcd";
            var counter = 100;
            var service = new TwoFactorAuthService();

            var otp = service.GenerateHotp(secretKey, counter);
            var result = service.VerifyHotp(secretKey, 100, otp);

            Assert.True(result);
        }
    }
}
