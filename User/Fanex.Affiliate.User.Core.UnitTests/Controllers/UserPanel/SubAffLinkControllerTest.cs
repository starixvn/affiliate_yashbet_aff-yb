using Fanex.Affiliate.User.Core.Controllers.UserPanel;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class SubAffLinkControllerTest
    {
        [Fact]
        public void Index_WhenCall_RedirectToRoute()
        {
            //Arrange
            var controller = new SubAffLinkController(null, null, null, null);

            //Action
            var result = controller.Index() as RedirectToRouteResult;

            //Assert
            Assert.Equal("UserPanel", result.RouteName);
            Assert.Equal("AffiliateLink", result.RouteValues["controller"]);
            Assert.False((bool)result.RouteValues["isTargetToMemberSite"]);
        }
    }
}
