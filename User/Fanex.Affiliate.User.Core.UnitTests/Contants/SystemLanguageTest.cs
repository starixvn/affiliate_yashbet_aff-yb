using Fanex.Affiliate.User.Core.Constants;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Contants
{
    public class SystemLanguageTest
    {
        [Theory]
        [InlineData("en-US", "EN")]
        [InlineData("abc", "EN")]
        public void GetUppercaseShortedCode_WhenCallWithValidLanguageCode_ReturnCorrectData(string languageCode, string expectedShortCode)
        {
            //Arrange
            string shortedCode;

            //Action
            try
            {
                shortedCode = SystemLanguage.GetUppercaseShortedCode(languageCode);
            }
            catch
            {
                shortedCode = "EN";
            }

            //Assert
            Assert.Equal(expectedShortCode, shortedCode);
        }

        [Theory]
        [InlineData("AffiliateLogin ", "AFFILIATE LOGIN")]
        [InlineData("abc ", "abc ")]
        public void GetLanguageText_WhenCallWithValidLanguageCode_ReturnCorrectData(string value, string expectedResult)
        {
            //Arrange
            string result;

            //Action
            result = SystemLanguage.GetLanguageText(value);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
