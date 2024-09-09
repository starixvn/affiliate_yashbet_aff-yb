using Autofac;
using Autofac.Integration.Mvc;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.DTO.Email;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Affiliate.Admin.Core.Repository.Customer;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Customer;
using Fanex.Affiliate.Admin.Core.Services.ThirdPartyLicensee;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class CustomerLeftPanelServiceTest
    {
        private ILifetimeScopeProvider scopeProvider;

        [Fact]
        public void GetAffiliateInfoOnLeftPanel_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.GetAffiliateInfoOnLeftPanel(It.IsAny<string>()));
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.GetAffiliateInfoOnLeftPanel("B2CAFF226", "", "", "");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAffiliateInfoOnLeftPanel_WhenCall_ReturnCorrectDataWithAsian()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.InternalAffiliateUserSiteURL).Returns("http://localhost:55690/Auth/AdminLogin");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.GetAffiliateInfoOnLeftPanel(It.IsAny<string>())).Returns(new AffiliateInfoOnLeftPanel { AffName = "B2CAFF226", AffType = AffType.Asian });
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.GetAffiliateInfoOnLeftPanel("B2CAFF226", "123", "234", "345");

            // Assert
            Assert.Equal("B2CAFF226", result.AffName);
            Assert.Equal(AffType.Asian, result.AffType);
        }

        [Fact]
        public void GetAffiliateInfoOnLeftPanel_WhenCall_ReturnCorrectDataWithEuropean()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.InternalAffiliateUserSiteURL).Returns("http://localhost:55690/Auth/AdminLogin");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.GetAffiliateInfoOnLeftPanel(It.IsAny<string>())).Returns(new AffiliateInfoOnLeftPanel { AffName = "B2CAFF226", AffType = AffType.European });
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.GetAffiliateInfoOnLeftPanel("B2CAFF226", "123", "234", "345");

            // Assert
            Assert.Equal("B2CAFF226", result.AffName);
            Assert.Equal(AffType.European, result.AffType);
        }

        [Fact]
        public void GetOnlineList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = "B2CAFF226";
            var passedParam = string.Empty;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.GetOnlineList(param)).Returns(new List<OnlineList> { new OnlineList() }).Callback<string>(p => passedParam = p);
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.GetOnlineList(param);

            // Assert
            Assert.Single(result);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetTransferLimitSetting_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = 123;
            var passedParam = 0;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.GetTransferLimitSetting(param)).Returns(new TransferLimitSettingInfo()).Callback<int>(p => passedParam = p);
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.GetTransferLimitSetting(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void UpdateTransferLimitSetting_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new TransferLimitSettingParam { TransferLimit = 100 };
            TransferLimitSettingParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateTransferLimitSetting(param)).Returns(new ResultInfo()).Callback<TransferLimitSettingParam>(p => passedParam = p);
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.UpdateTransferLimitSetting(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void UpdateTransferLimitSetting_WhenCallInvalidParam_ReturnError()
        {
            // Arrange
            var param = new TransferLimitSettingParam { DisabledTransfer = false };
            var service = CreateCustomerLeftPanelService();

            // Action
            var result = service.UpdateTransferLimitSetting(param);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Limit must be a whole positive number.", result.Message);
        }

        [Fact]
        public void UpdateAffType_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new UpdateAffTypeParam();
            UpdateAffTypeParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateAffType(param)).Returns(new ResultInfo()).Callback<UpdateAffTypeParam>(p => passedParam = p);
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.UpdateAffType(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void UpdateLanguage_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new UpdateLanguageParam();
            UpdateLanguageParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateLanguage(param)).Returns(new ResultInfo()).Callback<UpdateLanguageParam>(p => passedParam = p);
            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = service.UpdateLanguage(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateStatus_WhenCallWithDisapprovedEmail_ReturnCorrectDataWithDefaultLanguage()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.AffUserSiteURL).Returns("https://aff-yb.nexdev.net/YBUser");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var param = new UpdateStatusParam { AffId = 1271808, Status = "disapproved" };
            UpdateStatusParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateStatus(param)).Returns(new ResultInfo()).Callback<UpdateStatusParam>(p => passedParam = p);
            var affiliateService = new Mock<IAffiliateService>();
            affiliateService.Setup(aff => aff.GetAffiliateBasicInfo(It.IsAny<int>(), null)).Returns(new AffiliateBasicInfo { AffId = 1271808, AffType = 2 });

            var service = CreateCustomerLeftPanelService(affiliateService: affiliateService.Object, customerLeftPanelRepository: repository.Object);

            // Action
            var result = await service.UpdateStatus(param, new AffInformationEmail());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateStatus_WhenCallWithDisapprovedEmail_ReturnCorrectData()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.AffUserSiteURL).Returns("https://aff-yb.nexdev.net/YBUser");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var param = new UpdateStatusParam { AffId = 1271808, Status = "disapproved" };
            UpdateStatusParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateStatus(param)).Returns(new ResultInfo()).Callback<UpdateStatusParam>(p => passedParam = p);
            var affiliateService = new Mock<IAffiliateService>();
            affiliateService.Setup(aff => aff.GetAffiliateBasicInfo(It.IsAny<int>(), null)).Returns(new AffiliateBasicInfo { AffId = 1271808, AffType = 2, PreferredLanguage = "vi-VN" });

            var service = CreateCustomerLeftPanelService(affiliateService: affiliateService.Object, customerLeftPanelRepository: repository.Object);

            // Action
            var result = await service.UpdateStatus(param, new AffInformationEmail());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateStatus_WhenCallWithApproved_ReturnCorrectData()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.AffUserSiteURL).Returns("https://aff-yb.nexdev.net/YBUser");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var param = new UpdateStatusParam { AffId = 1271808, Status = "approved" };
            UpdateStatusParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateStatus(param)).Returns(new ResultInfo()).Callback<UpdateStatusParam>(p => passedParam = p);
            var affiliateService = new Mock<IAffiliateService>();
            affiliateService.Setup(aff => aff.GetAffiliateBasicInfo(It.IsAny<int>(), null)).Returns(new AffiliateBasicInfo { AffId = 1271808, AffType = 2, PreferredLanguage = "vi-VN" });
            var thirdPartyLicenseeService = new Mock<IThirdPartyLicenseeService>();
            thirdPartyLicenseeService.Setup(comm => comm.SendAffiliateInfo(It.IsAny<AffiliateBasicInfo>())).Returns(Task.CompletedTask);

            var service = CreateCustomerLeftPanelService(null, affiliateService.Object, repository.Object, thirdPartyLicenseeService.Object);

            // Action
            var result = await service.UpdateStatus(param, new AffInformationEmail());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateStatus_WhenCallWithApprovedAndException_ReturnCorrectData()
        {
            // Arrange
            MockingHelper.SetUpHttpContextSession();
            MockingHelper.SetUpLogEngineEnviroment();
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.AffUserSiteURL).Returns("https://aff-yb.nexdev.net/YBUser");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSetting.Object).As<IAppSetting>();
            var container = builder.Build();

            this.scopeProvider = new SimpleLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, scopeProvider);
            DependencyResolver.SetResolver(resolver);

            var param = new UpdateStatusParam { AffId = 1271808, Status = "approved" };
            UpdateStatusParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateStatus(param)).Returns(new ResultInfo()).Callback<UpdateStatusParam>(p => passedParam = p);
            var affiliateService = new Mock<IAffiliateService>();
            affiliateService.Setup(aff => aff.GetAffiliateBasicInfo(It.IsAny<int>(), null)).Returns(new AffiliateBasicInfo { AffId = 1271808, AffType = 2, PreferredLanguage = "vi-VN" });
            var thirdPartyLicenseeService = new Mock<IThirdPartyLicenseeService>();
            thirdPartyLicenseeService.Setup(comm => comm.SendAffiliateInfo(It.IsAny<AffiliateBasicInfo>())).Throws(new System.Exception());

            var service = CreateCustomerLeftPanelService(null, affiliateService.Object, repository.Object, thirdPartyLicenseeService.Object);

            // Action
            var result = await service.UpdateStatus(param, new AffInformationEmail());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateStatus_WhenCallWithDisapprovedEmail_ReturnError()
        {
            // Arrange
            var param = new UpdateStatusParam { AffId = 1271808, Status = "disapproved" };
            UpdateStatusParam passedParam = null;
            var repository = new Mock<ICustomerLeftPanelRepository>();
            repository.Setup(comm => comm.UpdateStatus(param)).Returns(new ResultInfo { Code = -999 }).Callback<UpdateStatusParam>(p => passedParam = p);

            var service = CreateCustomerLeftPanelService(customerLeftPanelRepository: repository.Object);

            // Action
            var result = await service.UpdateStatus(param, new AffInformationEmail());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        private static CustomerLeftPanelService CreateCustomerLeftPanelService(
            IEmailService emailService = null,
            IAffiliateService affiliateService = null,
            ICustomerLeftPanelRepository customerLeftPanelRepository = null,
            IThirdPartyLicenseeService thirdPartyLicenseeService = null)
        {
            var fakeEmailService = emailService ?? Mock.Of<IEmailService>();
            var fakeAffiliateService = affiliateService ?? Mock.Of<IAffiliateService>();
            var fakeCustomerLeftPanelRepository = customerLeftPanelRepository ?? Mock.Of<ICustomerLeftPanelRepository>();
            var fakeThirdPartyLicenseeService = thirdPartyLicenseeService ?? Mock.Of<IThirdPartyLicenseeService>();

            return new CustomerLeftPanelService(Mock.Of<IAppSetting>(), fakeEmailService, fakeAffiliateService, fakeCustomerLeftPanelRepository, fakeThirdPartyLicenseeService);
        }
    }
}
