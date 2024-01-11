using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Filters
{
    public class AjaxOnlyAttributeTest
    {
        [Fact]
        public void IsValidForRequest_WhenCall_ReturnCorrect()
        {
            var controller = NSubtituteMockingHelper.FakeController(new HomeController(null, null, null));
            NSubtituteMockingHelper.FakeAjaxRequest(controller.Request);

            var attr = new AjaxOnlyAttribute();

            var isValidForRequest = attr.IsValidForRequest(controller.ControllerContext, null);

            Assert.True(isValidForRequest);
        }
    }
}
