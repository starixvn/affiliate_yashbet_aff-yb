using Fanex.Affiliate.User.Core.Utilities;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Utilities
{
    public class SiteFriendlyNameHelperTest
    {
        [Theory]
        [InlineData("123", "abcxyz")]
        [InlineData("", "")]
        public void ReplaceTranslateVariable_WhenCall_ReturnCorrect(string value, string expectedResult)
        {
            //Arrange
            string result;

            //Action
            result = SiteFriendlyNameHelper.ReplaceTranslateVariable("abc{123}", value, "xyz");

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
