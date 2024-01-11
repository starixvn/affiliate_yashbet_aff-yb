using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class ViewLogControllerTest
    {
        [Fact]
        public void GetLogTypeList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var viewLogService = new Mock<IViewLogService>();
            viewLogService.Setup(s => s.GetLogTypeList()).Returns(new List<LogType> { new LogType { TypeId = 1, TypeName = "abc" } });
            var controller = CreateController(viewLogService: viewLogService.Object);

            // Action
            var result = controller.GetLogTypeList() as JsonNetResult;
            var logTypes = result.Data as IEnumerable<LogType>;

            // Assert
            Assert.Single(logTypes);
        }

        [Fact]
        public void GetViewLogList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new ViewLogParam { AffId = 1271808, LogType = 0, PageIndex = 1, PageSize = 20 };
            ViewLogParam passedParam = null;
            var viewLogService = new Mock<IViewLogService>();
            viewLogService.Setup(s => s.GetViewLogList(param)).Returns(new EntityResult<ViewLog> { MainData = new List<ViewLog> { new ViewLog() }, TotalItem = 1 }).Callback<ViewLogParam>(p => passedParam = p);
            var controller = CreateController(viewLogService: viewLogService.Object);

            // Action
            var result = controller.GetViewLogList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<ViewLog>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetViewLogList_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetViewLogList(new ViewLogParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private ViewLogController CreateController(
            ISessionService sessionService = null,
            IViewLogService viewLogService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeViewLogService = viewLogService ?? Mock.Of<IViewLogService>();

            return new ViewLogController(fakeSessionService, fakeViewLogService);
        }
    }
}
