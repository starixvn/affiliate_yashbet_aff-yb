using Fanex.Affiliate.User.Core.Controllers.UserPanel;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class SubAffMediaGalleryControllerTest
    {
        [Fact]
        public void Index_WhenCall_RedirectToRoute()
        {
            //Arrange
            var controller = new SubAffMediaGalleryController(null, null, null, null);

            //Action
            var result = controller.Index() as RedirectToRouteResult;

            //Assert
            Assert.Equal("UserPanel", result.RouteName);
            Assert.Equal("MediaGallery", result.RouteValues["controller"]);
            Assert.False((bool)result.RouteValues["isTargetToMemberSite"]);
        }
    }
}
