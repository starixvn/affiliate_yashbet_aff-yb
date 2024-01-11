using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Home;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Home;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateHomeController();

            // Action
            var viewResult = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("", viewResult.ViewName);
        }

        [Fact]
        public void GetGeneralInfo_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var homeService = new Mock<IHomeService>();
            homeService.Setup(t => t.GetGeneralInfo()).Returns(new GeneralInfo { PaymentRequestNumber = 1 });
            var controller = CreateHomeController(homeService: homeService.Object);

            // Action
            var result = controller.GetGeneralInfo() as JsonNetResult;
            var data = result.Data as GeneralInfo;

            // Assert
            Assert.Equal(1, data.PaymentRequestNumber);
        }

        [Fact]
        public void GetOnlineList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var homeService = new Mock<IHomeService>();
            homeService.Setup(t => t.GetOnlineList(1, 20)).Returns(new EntityResult<OnlineListInfo> { MainData = new List<OnlineListInfo> { new OnlineListInfo() } });
            var controller = CreateHomeController(homeService: homeService.Object);

            // Action
            var result = controller.GetOnlineList(1, 20) as JsonNetResult;
            var data = result.Data as EntityResult<OnlineListInfo>;

            // Assert
            Assert.Single(data.MainData);
        }

        private HomeController CreateHomeController(
            ISessionService sessionService = null,
            IHomeService homeService = null)
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeHomeService = homeService ?? Mock.Of<IHomeService>();

            return new HomeController(fakeSessionService, fakeHomeService);
        }
    }
}
