using Fanex.Affiliate.Admin.Core.Utilities;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Utilities
{
    public class SiteFriendlyNameHelperTest
    {
        [Fact]
        public void GetSize_WhenCall_ReturnCorrectData()
        {
            // Arrange

            // Action
            var result = SiteFriendlyNameHelper.ReplaceB2C(string.Empty);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}