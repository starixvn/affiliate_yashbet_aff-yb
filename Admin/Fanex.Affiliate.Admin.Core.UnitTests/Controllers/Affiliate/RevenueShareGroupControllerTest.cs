using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Controllers.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.CommGroup;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.Utilities.CommGroup;
using Fanex.Affiliate.Admin.Core.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Affiliate
{
    public class RevenueShareGroupControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(p => p.GetProductList()).Returns(new List<Product> { new Product { ProductId = 123, ProductName = "abc" } });

            var commGroupCommonService = new Mock<ICommGroupCommonService>();
            commGroupCommonService.Setup(c => c.GetDeductionList()).Returns(new List<DTO.CommissionGroup.DeductionItem> { new DTO.CommissionGroup.DeductionItem { DeductionId = 13, DeductionName = "ExchangeGgrShare", DeductedPercentage = 0.05m } });

            var controller = CreateCommissionGroupController(productRepository: productRepository.Object, commGroupCommonService: commGroupCommonService.Object);

            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Content(It.IsAny<string>())).Returns("/Portal");

            controller.Url = urlHelperMock.Object;

            // Action
            var viewResult = controller.Index() as ViewResult;
            var viewName = viewResult.ViewName;
            var model = viewResult.Model as CommissionGroupViewModel;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Affiliate/CommissionGroup.cshtml", viewName);
            Assert.Single(model.ProductList);
            Assert.Single(model.DeductionList);
        }

        [Fact]
        public void GetCommissionGroupList_GetData_ReturnCorrectData()
        {
            // Arrange
            var param = new GetCommissionGroupParam { GroupName = "abc", PageIndex = 1, PageSize = 20 };
            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.GetCommissionGroupList(param)).Returns(new List<CommissionGroupSetting> { new CommissionGroupSetting { GroupName = "abc" } }).Callback<GetCommissionGroupParam>(p => param.Nrows = 1);
            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object);

            // Action
            var result = controller.GetCommissionGroupList(param) as JsonNetResult;
            var model = result.Data as GetCommissionGroupViewModel;

            // Assert
            Assert.Equal(1, model.TotalRows);
            Assert.Single(model.CommissionSettingList);
        }

        [Fact]
        public void GetCommissionGroup_GetData_ReturnCorrectData()
        {
            // Arrange
            var param = new GetCommissionGroupParam { GroupId = 123 };
            GetCommissionGroupParam passedParam = null;
            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.GetCommissionGroupList(param))
                .Returns(new List<CommissionGroupSetting> { new CommissionGroupSetting { GroupId = 123 } })
                .Callback<GetCommissionGroupParam>(p => passedParam = p);
            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object);

            // Action
            var result = controller.GetCommissionGroup(param) as JsonNetResult;
            var model = result.Data as CommissionGroupSetting;

            // Assert
            Assert.Equal(param.GroupId, model.GroupId);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetCommissionGroup_GetData_ReturnNullData()
        {
            // Arrange
            var param = new GetCommissionGroupParam { GroupId = 123 };
            GetCommissionGroupParam passedParam = null;
            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.GetCommissionGroupList(param)).Returns(new List<CommissionGroupSetting>()).Callback<GetCommissionGroupParam>(p => passedParam = p);
            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object);

            // Action
            var result = controller.GetCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(CommGroupErrorMessage.InvalidGroupId, resultInfo.Message);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetCommissionGroup_GetDataWithInvalidParam_ReturnError()
        {
            // Arrange
            var param = new GetCommissionGroupParam();
            var controller = CreateCommissionGroupController();
            var expectedResult = new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode,
                Message = CommGroupErrorMessage.InvalidGroupId
            };

            // Action
            var result = controller.GetCommissionGroup(param) as JsonNetResult;
            var model = result.Data as ResultInfo;

            // Assert
            Assert.Equal(expectedResult.Code, model.Code);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCallWithInvalidModelState_ExcuteIncorrect()
        {
            // Arrange
            const string ErrorMessage1 = "ErrorMessage1";
            var param = new CommGroupSettingParam();
            var controller = CreateCommissionGroupController();
            controller.ModelState.AddModelError("Error01", ErrorMessage1);

            // Action
            var result = controller.CreateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(ErrorMessage1, resultInfo.Message);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCallWithEmptyGroupName_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();
            param.GroupName = string.Empty;

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.CreateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCallWithGroupNameMoreThan100Characters_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();
            param.GroupName = "Create Commission Group When Call With Group Name More Than 100 Characters, Create Commission Group When Call With Group Name More Than 100 Characters";

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.CreateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCallWithInvalidPlatformFee_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();
            param.GroupName = "abc";

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.CreateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CreateAndUpdateCommissionGroup_WhenCallWithInvalidGroupName_ExcuteIncorrect(bool isInsert)
        {
            // Arrange
            var param = isInsert ? CreateCommGroupSettingParam() : CreateUpdateCommGroupSettingParam();
            param.GroupName = string.Empty;

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = (isInsert ? controller.CreateCommissionGroup(param) : controller.UpdateCommissionGroup(param)) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCallWithValidParam_ExcuteCorrectly()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();

            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.CreateCommissionGroup(It.IsAny<CreateUpdateCommissionGroupParam>())).Returns(new ResultInfo());

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object, commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.CreateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCallWithValidParamAndDescriptionNull_ExcuteCorrectly()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();
            param.Description = null;

            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.CreateCommissionGroup(It.IsAny<CreateUpdateCommissionGroupParam>())).Returns(new ResultInfo());

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object, commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.CreateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithInvalidModelState_ExcuteIncorrect()
        {
            // Arrange
            const string ErrorMessage1 = "ErrorMessage1";
            var param = CreateUpdateCommGroupSettingParam();
            var controller = CreateCommissionGroupController();
            controller.ModelState.AddModelError("Error01", ErrorMessage1);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(ErrorMessage1, resultInfo.Message);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithInvalidGroupId_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithGroupNameMoreThan100Characters_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateUpdateCommGroupSettingParam();
            param.GroupName = "Update Commission Group When Call With Group Name More Than 100 Characters, Update Commission Group When Call With Group Name More Than 100 Characters";

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithInvalidPlatformFee_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateUpdateCommGroupSettingParam();
            param.GroupName = "abc";

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithInvalidGroupNameAndPlatformFee_ExcuteIncorrect()
        {
            // Arrange
            var param = CreateCommGroupSettingParam();
            param.GroupName = string.Empty;

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithValidParam_ExcuteCorrectly()
        {
            // Arrange
            var param = CreateUpdateCommGroupSettingParam();

            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.UpdateCommissionGroup(It.IsAny<CreateUpdateCommissionGroupParam>())).Returns(new ResultInfo());

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object, commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCallWithValidParamAndDescriptionNull_ExcuteCorrectly()
        {
            // Arrange
            var param = CreateUpdateCommGroupSettingParam();
            param.Description = null;

            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.UpdateCommissionGroup(It.IsAny<CreateUpdateCommissionGroupParam>())).Returns(new ResultInfo());

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object, commGroupValidation: commGroupValidation.Object);

            // Action
            var result = controller.UpdateCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        [Fact]
        public void DeleteCommissionGroup_WhenCallWithInvalidModelState_ExcuteIncorrect()
        {
            // Arrange
            const string ErrorMessage1 = "ErrorMessage1";
            var param = new DeleteCommissionGroupParam();
            var controller = CreateCommissionGroupController();
            controller.ModelState.AddModelError("Error01", ErrorMessage1);

            // Action
            var result = controller.DeleteCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(ErrorMessage1, resultInfo.Message);
        }

        [Fact]
        public void DeleteCommissionGroup_WhenCallWithValidParam_ExcuteCorrectly()
        {
            // Arrange
            var param = new DeleteCommissionGroupParam();
            var commissionGroupService = new Mock<ICommissionGroupService>();
            commissionGroupService.Setup(c => c.DeleteCommissionGroup(param)).Returns(new ResultInfo());
            var controller = CreateCommissionGroupController(commissionGroupService: commissionGroupService.Object);

            // Action
            var result = controller.DeleteCommissionGroup(param) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        private static RevenueGroupController CreateCommissionGroupController(
            ISessionService sessionService = null,
            ICommissionGroupService commissionGroupService = null,
            IProductRepository productRepository = null,
            ICommGroupCommonService commGroupCommonService = null,
            ICommGroupValidation commGroupValidation = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeCommissionGroupService = commissionGroupService ?? Mock.Of<ICommissionGroupService>();
            var fakeProductRepository = productRepository ?? Mock.Of<IProductRepository>();
            var fakecommGroupCommonService = commGroupCommonService ?? Mock.Of<ICommGroupCommonService>();
            var fakeCommGroupValidation = commGroupValidation ?? Mock.Of<ICommGroupValidation>();
            var controller = new RevenueGroupController(fakeSessionService, fakeCommissionGroupService, fakeProductRepository, fakecommGroupCommonService, fakeCommGroupValidation);

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

        private static CommGroupSettingParam CreateCommGroupSettingParam()
        {
            return new CommGroupSettingParam
            {
                Description = "Test",
                GroupId = 0,
                GroupName = "Test"
            };
        }

        private static CommGroupSettingParam CreateUpdateCommGroupSettingParam()
        {
            var CommGroupSettingParam = CreateCommGroupSettingParam();
            CommGroupSettingParam.GroupId = 123;

            return CommGroupSettingParam;
        }
    }
}
