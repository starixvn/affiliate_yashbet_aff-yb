using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class AdjustmentControllerTest
    {
        [Fact]
        public void GetBalanceBroughtForward_WhenCallWithValidParam_ExcuteCorrect()
        {
            // Arrange
            var param = 1271808;
            var passedParam = 0;
            var adjustmentService = new Mock<IAdjustmentService>();
            adjustmentService.Setup(s => s.GetBalanceBroughtForward(param)).Returns(123m).Callback<int>(p => passedParam = p);
            var controller = CreateAdjustmentController(adjustmentService: adjustmentService.Object);

            // Action
            var result = controller.GetBalanceBroughtForward(param) as JsonNetResult;

            // Assert
            Assert.Equal(123m, result.Data);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetBalanceBroughtForward_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateAdjustmentController();

            // Action
            var result = controller.GetBalanceBroughtForward(0) as JsonNetResult;

            // Assert
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public void InsertAdjustment_WhenCallWithValidParam_ExcuteCorrect()
        {
            // Arrange
            var param = new AdjustmentParam { AffId = 1271808, Amount = 123 };
            AdjustmentParam passedParam = null;
            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.CheckPermission(It.IsAny<CheckPermissionParam>())).Returns(new ResultInfo());

            var adjustmentService = new Mock<IAdjustmentService>();
            adjustmentService.Setup(s => s.GetBalanceBroughtForward(param.AffId)).Returns(1234);
            adjustmentService.Setup(s => s.InsertAdjustment(It.IsAny<AdjustmentParam>())).Returns(new ResultInfo()).Callback<AdjustmentParam>(p => passedParam = p);
            var controller = CreateAdjustmentController(null, adjustmentService.Object, authRepository.Object);

            // Action
            var result = controller.InsertAdjustment(param) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.True(data.IsSuccessful);
            Assert.Equal(param.AffId, passedParam.AffId);
        }

        [Fact]
        public void InsertAdjustment_WhenCallWithInvalidParam_ExcuteCorrect()
        {
            // Arrange
            var param = new AdjustmentParam { AffId = 1271808, Amount = 123 };
            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.CheckPermission(It.IsAny<CheckPermissionParam>())).Returns(new ResultInfo());

            var adjustmentService = new Mock<IAdjustmentService>();
            adjustmentService.Setup(s => s.GetBalanceBroughtForward(param.AffId)).Returns(922337203685999m);
            var controller = CreateAdjustmentController(null, adjustmentService.Object, authRepository.Object);
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertAdjustment(param) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.False(data.IsSuccessful);
            Assert.Equal("The amount is invalid", data.Message);
        }

        [Fact]
        public void InsertAdjustment_WhenCallWithInvalidAffId_ReturnError()
        {
            // Arrange
            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.CheckPermission(It.IsAny<CheckPermissionParam>())).Returns(new ResultInfo());

            var controller = CreateAdjustmentController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.InsertAdjustment(new AdjustmentParam()) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.False(data.IsSuccessful);
            Assert.Equal("Affiliate not exists!", data.Message);
        }

        [Fact]
        public void InsertAdjustment_WhenCallWithNotHavePermission_ReturnError()
        {
            // Arrange
            var param = new AdjustmentParam { AffId = 1271808, Amount = 123 };
            var authRepository = new Mock<IAuthRepository>();
            authRepository.Setup(a => a.CheckPermission(It.IsAny<CheckPermissionParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });
            var controller = CreateAdjustmentController(null, null, authRepository.Object);

            // Action
            var result = controller.InsertAdjustment(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        private AdjustmentController CreateAdjustmentController(
            ISessionService sessionService = null,
            IAdjustmentService adjustmentService = null,
            IAuthRepository authRepository = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeAdjustmentService = adjustmentService ?? Mock.Of<IAdjustmentService>();
            var fakeAuthRepository = authRepository ?? Mock.Of<IAuthRepository>();

            var controller = new AdjustmentController(fakeSessionService, fakeAdjustmentService, fakeAuthRepository);

            var request = MockingHelper.GetHttpRequest();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => $"Encoded{s}");

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Session.SessionID == "SessionId"
            );

            return controller;
        }
    }
}
