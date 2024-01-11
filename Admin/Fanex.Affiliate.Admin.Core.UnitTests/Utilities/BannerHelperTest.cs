using Fanex.Affiliate.Admin.Core.Utilities.MediaGallery;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Utilities
{
    public class BannerHelperTest
    {
        [Fact]
        public void GetSize_WhenCall_ReturnCorrectData()
        {
            // Arrange

            // Action
            var result = BannerHelper.GetSize(100, 150);

            // Assert
            Assert.Equal("100x150", result);
        }
    }
}