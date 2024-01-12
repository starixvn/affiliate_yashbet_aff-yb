using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Api;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class UserAccountServiceTest
    {
        [Fact]
        public void GetUserAccountInfo_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = 1271808;
            var passedParam = 0;
            var repository = new Mock<IUserAccountRepository>();
            repository.Setup(comm => comm.GetUserAccountInfo(param)).Returns(new UserAccount { AffId = 1271808 }).Callback<int>(p => passedParam = p);
            var service = CreateUserAccountService(repository.Object);

            // Action
            var result = service.GetUserAccountInfo(param);

            // Assert
            Assert.Equal(param, result.AffId);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithValid_ExcuteSuccessfully()
        {
            // Arrange
            var param = GetUserAccountParam();
            UserAccountParam passedParam = null;
            var repository = new Mock<IUserAccountRepository>();
            repository.Setup(comm => comm.UpdateUserAccount(param)).Returns(new ResultInfo()).Callback<UserAccountParam>(p => passedParam = p);
            var service = CreateUserAccountService(repository.Object);

            // Action
            var result = await service.UpdateUserAccountAsync(param);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithValidPlayerAccount_ExcuteSuccessfully()
        {
            // Arrange
            var repository = new Mock<IUserAccountRepository>();
            repository.Setup(r => r.GetUserAccountInfo(It.IsAny<int>())).Returns(new UserAccount { PlayerUsername = "Abc" });
            var licenseeService = new Mock<ILicenseeService>();
            licenseeService.Setup(s => s.CheckCustomerIsValid(It.IsAny<MemberSiteCustomerInfo>()))
                .Returns(new ResultInfo { Code = 0 });
            licenseeService.Setup(s => s.GetMemberSiteCustomerInfo(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new MemberSiteCustomerInfo());
            repository.Setup(comm => comm.UpdateUserAccount(It.IsAny<UserAccountParam>())).Returns(new ResultInfo());
            var service = CreateUserAccountService(repository.Object, licenseeService.Object);

            // Action
            var result = await service.UpdateUserAccountAsync(new UserAccountParam { PreferredPayment = "Player", PlayerUsername = "123", NoSiteDescription = "abc" });

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithInvalidPlayerAccount_ReturnError()
        {
            // Arrange
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(ap => ap.SiteFriendlyName).Returns("YB");
            var repository = new Mock<IUserAccountRepository>();
            repository.Setup(r => r.GetUserAccountInfo(It.IsAny<int>())).Returns(new UserAccount { PlayerUsername = "Abc" });
            var licenseeService = new Mock<ILicenseeService>();
            licenseeService.Setup(s => s.CheckCustomerIsValid(It.IsAny<MemberSiteCustomerInfo>())).Returns(new ResultInfo { Code = 100 });

            var service = CreateUserAccountService(repository.Object, licenseeService.Object, appSetting.Object);

            // Action
            var result = await service.UpdateUserAccountAsync(new UserAccountParam { PreferredPayment = "Player", PlayerUsername = "123", NoSiteDescription = "abc" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(100, result.Code);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithInvalidNoSiteDescription_ReturnError()
        {
            // Arrange
            var service = CreateUserAccountService();

            // Action
            var result = await service.UpdateUserAccountAsync(new UserAccountParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Please describe how you would like to promote  if you don't have the URL is required", result.Message);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithInvalidSiteUrl_ReturnError()
        {
            // Arrange
            var service = CreateUserAccountService();

            // Action
            var result = await service.UpdateUserAccountAsync(new UserAccountParam { HasWebSite = true });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Website URL is required", result.Message);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithInvalidPaymentMethod_ReturnError()
        {
            // Arrange
            var service = CreateUserAccountService();

            // Action
            var result = await service.UpdateUserAccountAsync(new UserAccountParam { PreferredPayment = "Wire", NoSiteDescription = "abc" });

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Update is failed!", result.Message);
        }

        private static UserAccountService CreateUserAccountService(
            IUserAccountRepository userAccountRepository = null,
            ILicenseeService licenseeService = null,
            IAppSetting appSeting = null)
        {
            var fakeUserAccountRepository = userAccountRepository ?? Mock.Of<IUserAccountRepository>();
            var fakeAppSetting = appSeting ?? Mock.Of<IAppSetting>();
            var fakeLicenseeService = licenseeService ?? Mock.Of<ILicenseeService>();

            return new UserAccountService(fakeUserAccountRepository, fakeLicenseeService, fakeAppSetting);
        }

        private static UserAccountParam GetUserAccountParam()
        {
            return new UserAccountParam
            {
                AffId = 1271808,
                FirstName = "Hinh",
                LastName = "Lê Văn",
                Gender = false,
                Email = "abc@gmail.com",
                Phone = "01234567899",
                ContactApps = "[{'Name':'MSN / YM / QQ','Value':'@123 Lê Văn','Order':0}]",
                Address = "Test",
                City = "test",
                State = "700000",
                Postcode = "700000",
                Country = "Viet Nam",
                MoreSiteUrls = "www.sportsbookie.com",
                SiteUrl = "https://aff-yb.nexdev.net/YBUser/UserPanel/Main/#MyAccount",
                PreferredPayment = "Wire",
                PlayerUsername = "bettingexp",
                NoSiteDescription = "test",
                NetellerAccountId = "1",
                MoneybookersEmail = "abc@gmail.com",
                BankName = "Test",
                BankAddress = "test",
                BankCode = "test",
                BankAccountName = "test",
                BankAccountNumber = "test",
                MailOption = false,
                ContactAppList = new List<ContactApp> { new ContactApp { Name = "MSN/YM/QQ", Value = "@123 Lê Văn", Order = 0 } }
            };
        }
    }
}
