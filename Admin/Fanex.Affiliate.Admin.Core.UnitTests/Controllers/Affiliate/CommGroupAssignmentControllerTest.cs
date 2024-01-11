using Autofac;
using Autofac.Integration.Mvc;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Controllers.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.CommGroup;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Affiliate.Admin.Core.Utilities.CommGroup;
using Fanex.Affiliate.Admin.Core.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Affiliate
{
    public class CommGroupAssignmentControllerTest
    {
        private ILifetimeScopeProvider scopeProvider;

        [Fact]
        public void AssignGroup_WhenCallWithInvalidAffId_ReturnViewWithEmptyModel()
        {
            // Arrange
            var controller = CreateCommGroupAssignmentController();

            // Action
            var viewResult = controller.AssignGroup(0) as ViewResult;
            var model = viewResult.Model as AssignNormalGroupViewModel;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Affiliate/AssignGroup.cshtml", viewResult.ViewName);
            Assert.Equal(0, model.SelectedGroup);
        }

        [Fact]
        public void AssignGroup_WhenCall_ReturnView()
        {
            // Arrange
            int affId = 1271808;
            GetGroupListParam passedParam = null;
            var commGroupList = new List<SelectItem> { new SelectItem { Id = "133", Name = "Test" } };
            var commGroupAssignmentService = new Mock<ICommGroupAssignmentService>();
            commGroupAssignmentService.Setup(c => c.GetNormalCommGroupList(It.IsAny<GetGroupListParam>())).Returns(commGroupList).Callback<GetGroupListParam>(p => passedParam = p);
            var affiliateService = new Mock<IAffiliateService>();
            affiliateService.Setup(a => a.GetAffiliateBasicInfo(affId, null)).Returns(new AffiliateBasicInfo { AffId = affId });

            var controller = CreateCommGroupAssignmentController(affiliateService: affiliateService.Object, commGroupAssignmentService: commGroupAssignmentService.Object);

            // Action
            var viewResult = controller.AssignGroup(affId) as ViewResult;
            var model = viewResult.Model as AssignNormalGroupViewModel;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Affiliate/AssignGroup.cshtml", viewResult.ViewName);
            Assert.Equal(0, model.SelectedGroup);
            Assert.Equal(affId, passedParam.AffId);
        }

        [Fact]
        public async Task AssignGroup_WhenCallPostWithInvalidParam_ReturnError()
        {
            // Arrange
            var assignParam = new AssignGroupParam { AffId = 1271808, GroupId = 133 };
            var affInfo = new AffInfoParam();
            const string ErrorMessage1 = "ErrorMessage1";
            var controller = CreateCommGroupAssignmentController();
            controller.ModelState.AddModelError("Error01", ErrorMessage1);
            var expectResult = new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode,
                Message = ErrorMessage1
            };
            // Action
            var jsonNetResult = await controller.AssignGroup(assignParam, affInfo) as JsonNetResult;
            var resultInfo = jsonNetResult.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(expectResult.Code, resultInfo.Code);
            Assert.Equal(expectResult.Message, resultInfo.Message);
        }

        [Fact]
        public async Task AssignGroup_WhenCallPostWithInvalidGroupId_ReturnError()
        {
            // Arrange
            var assignParam = new AssignGroupParam { AffId = 1271808 };
            var affInfo = new AffInfoParam();
            var controller = CreateCommGroupAssignmentController();
            var expectResult = new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode
            };
            // Action
            var jsonNetResult = await controller.AssignGroup(assignParam, affInfo) as JsonNetResult;
            var resultInfo = jsonNetResult.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(expectResult.Code, resultInfo.Code);
        }

        [Fact]
        public async Task AssignGroup_WhenCallPost_ExcuteCorrectly()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("UAT");
            appSetting.Setup(a => a.IsProduction).Returns(true);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var assignParam = new AssignGroupParam { AffId = 1271808, GroupId = 133 };
            var affInfo = new AffInfoParam();
            var commGroupAssignmentService = new Mock<ICommGroupAssignmentService>();
            commGroupAssignmentService.Setup(c => c.AssignGroup(It.IsAny<AssignGroupParam>(), affInfo, It.IsAny<AffInformationEmail>())).Returns(Task.FromResult(new ResultInfo()));
            var controller = CreateCommGroupAssignmentController(commGroupAssignmentService: commGroupAssignmentService.Object);

            // Action
            var jsonNetResult = await controller.AssignGroup(assignParam, affInfo) as JsonNetResult;
            var resultInfo = jsonNetResult.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        [Fact]
        public void AssignPrivateGroup_WhenCallWithInvalidAffId_ReturnViewWithEmptyModel()
        {
            // Arrange
            var controller = CreateCommGroupAssignmentController();

            // Action
            var viewResult = controller.AssignPrivateGroup(0) as ViewResult;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Affiliate/AssignPrivateGroup.cshtml", viewResult.ViewName);
        }

        [Fact]
        public void AssignPrivateGroup_WhenCall_ReturnView()
        {
            // Arrange
            int affId = 1271808;
            var productList = new List<Product> { new Product { ProductId = 123, ProductName = "Casino", IsP2P = true } };
            var deductionList = new List<DeductionItem> { new DeductionItem { DeductionId = 13, DeductionName = "ExchangeGgrShare", DeductedPercentage = 0.05m, IsDeductedPercent = true, DeductedAmount = "" } };
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(p => p.GetProductList()).Returns(productList);
            var commGroupCommonService = new Mock<ICommGroupCommonService>();
            commGroupCommonService.Setup(c => c.GetDeductionList()).Returns(deductionList);

            var controller = CreateCommGroupAssignmentController(productRepository: productRepository.Object, commGroupCommonService: commGroupCommonService.Object);
            var expectModel = new AssignPrivateGroupViewModel
            {
                ProductList = productList,
                DeductionList = deductionList
            };
            // Action
            var viewResult = controller.AssignPrivateGroup(affId) as ViewResult;
            var model = viewResult.Model as AssignPrivateGroupViewModel;

            // Assert
            Assert.Equal("~/Areas/Portal/Views/Affiliate/AssignPrivateGroup.cshtml", viewResult.ViewName);
            Assert.Equal(expectModel.ProductList, model.ProductList);
            Assert.Equal(expectModel.DeductionList, model.DeductionList);
        }

        [Fact]
        public async Task AssignPrivateGroup_WhenCallPostWithInvalidParam_ReturnError()
        {
            // Arrange
            var commGroup = new CommGroupSettingParam();
            var affInfo = new AffInfoParam();
            const string ErrorMessage1 = "ErrorMessage1";
            var controller = CreateCommGroupAssignmentController();
            controller.ModelState.AddModelError("Error01", ErrorMessage1);
            var expectResult = new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode,
                Message = ErrorMessage1
            };
            // Action
            var jsonNetResult = await controller.AssignPrivateGroup(commGroup, affInfo) as JsonNetResult;
            var resultInfo = jsonNetResult.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
            Assert.Equal(expectResult.Code, resultInfo.Code);
            Assert.Equal(expectResult.Message, resultInfo.Message);
        }

        [Fact]
        public async Task AssignPrivateGroup_WhenCallWithInvalidPlatformFee_ExcuteIncorrect()
        {
            // Arrange
            var commGroup = CreateCommGroupSettingParam();
            var affInfo = new AffInfoParam { AffId = 1271808 };
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(p => p.GetProductList()).Returns(new List<Product>
            {
                new Product { ProductId = 1 },
                new Product { ProductId = 2 },
                new Product { ProductId = 3 }
            });

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo { Code = ErrCode.GeneralErrorCode });

            var controller = CreateCommGroupAssignmentController(productRepository: productRepository.Object, commGroupValidation: commGroupValidation.Object);

            // Action
            var result = await controller.AssignPrivateGroup(commGroup, affInfo) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.False(resultInfo.IsSuccessful);
        }

        [Fact]
        public async Task AssignPrivateGroup_WhenCall_ExcuteCorrectly()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("LOCAL");
            appSetting.Setup(a => a.IsProduction).Returns(false);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var commGroup = CreateCommGroupSettingParam();
            var affInfo = new AffInfoParam { AffId = 1271808 };
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(p => p.GetProductList()).Returns(new List<Product>
            {
                new Product { ProductId = 1 },
                new Product { ProductId = 3 },
                new Product { ProductId = 4 }
            });
            var commGroupAssignmentService = new Mock<ICommGroupAssignmentService>();
            commGroupAssignmentService.Setup(c => c.AssignPrivateGroup(It.IsAny<CreateUpdateCommissionGroupParam>(), affInfo, It.IsAny<AffInformationEmail>())).Returns(Task.FromResult(new ResultInfo()));

            var commGroupValidation = new Mock<ICommGroupValidation>();
            commGroupValidation.Setup(e => e.ValidateCommGroup(It.IsAny<CommGroupSettingParam>())).Returns(new ResultInfo());

            var controller = CreateCommGroupAssignmentController(productRepository: productRepository.Object, commGroupAssignmentService: commGroupAssignmentService.Object, commGroupValidation: commGroupValidation.Object);

            // Action
            var result = await controller.AssignPrivateGroup(commGroup, affInfo) as JsonNetResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.True(resultInfo.IsSuccessful);
        }

        private static CommGroupAssignmentController CreateCommGroupAssignmentController(
            ISessionService sessionService = null,
            IProductRepository productRepository = null,
            IAffiliateService affiliateService = null,
            ICommGroupAssignmentService commGroupAssignmentService = null,
            ICommGroupCommonService commGroupCommonService = null,
            ICommGroupValidation commGroupValidation = null)
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeProductRepository = productRepository ?? Mock.Of<IProductRepository>();
            var fakeAffiliateService = affiliateService ?? Mock.Of<IAffiliateService>();
            var fakeCommGroupAssignmentService = commGroupAssignmentService ?? Mock.Of<ICommGroupAssignmentService>();
            var fakeCommGroupCommonService = commGroupCommonService ?? Mock.Of<ICommGroupCommonService>();
            var fakeCommGroupValidation = commGroupValidation ?? Mock.Of<ICommGroupValidation>();

            var controller = new CommGroupAssignmentController(fakeSessionService, fakeProductRepository, fakeAffiliateService, fakeCommGroupAssignmentService, fakeCommGroupCommonService, fakeCommGroupValidation);

            var request = MockingHelper.GetHttpRequest();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => $"Encoded{s}");

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Session.SessionID == "SessionId"
            );

            var expectedPath = "~/";
            var expectedContent = "/";
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock
                .Setup(m => m.Content(expectedPath))
                .Returns(expectedContent);

            controller.Url = urlHelperMock.Object;

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
    }
}
