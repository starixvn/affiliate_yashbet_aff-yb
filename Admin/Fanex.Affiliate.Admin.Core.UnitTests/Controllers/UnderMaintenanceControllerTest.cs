using Fanex.Affiliate.Admin.Core.Controllers;
using Fanex.Affiliate.Admin.Core.Services.UnderMaintenanceService;
using Moq;
using ONEbook.UM.Models;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers
{
    public class UnderMaintenanceControllerTest
    {
        [Fact]
        public void Index_WhenCall_RedirectToLogin()
        {
            // Arrange
            var underMaintenanceService = new Mock<IUnderMaintenanceService>();
            underMaintenanceService.Setup(u => u.IsUnderMaintenance()).Returns(false);
            var controller = CreateUnderMaintenanceController(underMaintenanceService.Object);

            // Action
            var viewResult = controller.Index() as RedirectResult;

            // Assert
            Assert.Equal("~/Portal/Login", viewResult.Url);
        }

        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var underMaintenanceService = new Mock<IUnderMaintenanceService>();
            underMaintenanceService.Setup(u => u.IsUnderMaintenance()).Returns(true);
            underMaintenanceService.Setup(u => u.GetUnderMaintenanceTime()).Returns(new UnderMaintenanceTime { From = "123", To = "234" });
            var controller = CreateUnderMaintenanceController(underMaintenanceService.Object);

            // Action
            var viewResult = controller.Index() as ViewResult;
            var model = viewResult.Model as UnderMaintenanceTime;

            // Assert
            Assert.Equal("", viewResult.ViewName);
            Assert.Equal("123", model.From);
            Assert.Equal("234", model.To);
        }

        private UnderMaintenanceController CreateUnderMaintenanceController(
            IUnderMaintenanceService underMaintenanceService = null
            )
        {
            var fakeUnderMaintenanceService = underMaintenanceService ?? Mock.Of<IUnderMaintenanceService>();

            return new UnderMaintenanceController(fakeUnderMaintenanceService);
        }
    }
}
