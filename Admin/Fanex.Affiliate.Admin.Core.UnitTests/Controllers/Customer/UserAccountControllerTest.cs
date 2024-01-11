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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class UserAccountControllerTest
    {
        private const int affId = 1271808;

        [Fact]
        public void GetUserAccountInfo_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var passedParam = 0;
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(s => s.GetUserAccountInfo(affId)).Returns(GetUserAccount()).Callback<int>(p => passedParam = p);
            var controller = CreateUserAccountController(userAccountService: userAccountService.Object);

            // Action
            var result = controller.GetUserAccountInfo(affId) as JsonNetResult;
            var item = result.Data as UserAccount;

            // Assert
            Assert.Equal(affId, item.AffId);
            Assert.Equal(affId, passedParam);
        }

        [Fact]
        public void GetUserAccountInfo_WhenCallWithValidParam_ExcuteWithCorrectDataWithContactAppsIsNull()
        {
            // Arrange
            var userAccount = GetUserAccount();
            userAccount.ContactApps = "";
            var passedParam = 0;
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(s => s.GetUserAccountInfo(affId)).Returns(userAccount).Callback<int>(p => passedParam = p);
            var controller = CreateUserAccountController(userAccountService: userAccountService.Object);

            // Action
            var result = controller.GetUserAccountInfo(affId) as JsonNetResult;
            var item = result.Data as UserAccount;

            // Assert
            Assert.Equal(affId, item.AffId);
            Assert.Equal(affId, passedParam);
        }

        [Fact]
        public void GetUserAccountInfo_WhenCallWithValidParam_ReturnNull()
        {
            // Arrange
            var controller = CreateUserAccountController();

            // Action
            var result = controller.GetUserAccountInfo(affId) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetUserAccountInfo_WhenCallWithInvalidParam_ReturnNull()
        {
            // Arrange
            var controller = CreateUserAccountController();

            // Action
            var result = controller.GetUserAccountInfo(0) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithValidParam_ExcuteSuccessfully()
        {
            // Arrange
            var param = GetUserAccountParam();
            UserAccountParam passedParam = null;
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(s => s.UpdateUserAccountAsync(It.IsAny<UserAccountParam>()))
                .Returns(Task.FromResult(new ResultInfo()))
                .Callback<UserAccountParam>(p => passedParam = p);
            var controller = CreateUserAccountController(userAccountService: userAccountService.Object);

            // Action
            var result = await controller.UpdateUserAccount(param) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.True(data.IsSuccessful);
            Assert.Equal(param.AffId, passedParam.AffId);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithValidParam_ExcuteSuccessfullyWithContactAppListIsNull()
        {
            // Arrange
            var param = GetUserAccountParam();
            param.ContactAppList = null;
            UserAccountParam passedParam = null;
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(s => s.UpdateUserAccountAsync(It.IsAny<UserAccountParam>()))
                .Returns(Task.FromResult(new ResultInfo()))
                .Callback<UserAccountParam>(p => passedParam = p);
            var controller = CreateUserAccountController(userAccountService: userAccountService.Object);

            // Action
            var result = await controller.UpdateUserAccount(param) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.True(data.IsSuccessful);
            Assert.Equal(param.AffId, passedParam.AffId);
        }

        [Fact]
        public async Task UpdateUserAccount_WhenCallWithInvalidParam_ReturnError()
        {
            // Arrange
            var controller = CreateUserAccountController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = await controller.UpdateUserAccount(new UserAccountParam()) as JsonNetResult;
            var data = result.Data as ResultInfo;

            // Assert
            Assert.False(data.IsSuccessful);
            Assert.Equal("", data.Message);
        }

        private UserAccountController CreateUserAccountController(
            ISessionService sessionService = null,
            IUserAccountService userAccountService = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeUserAccountService = userAccountService ?? Mock.Of<IUserAccountService>();

            var controller = new UserAccountController(fakeSessionService, fakeUserAccountService);

            var request = MockingHelper.GetHttpRequest();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => s);
            server.Setup(s => s.HtmlDecode(It.IsAny<string>())).Returns<string>(s => s);

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Session.SessionID == "SessionId"
            );

            return controller;
        }

        private static UserAccount GetUserAccount()
        {
            return new UserAccount
            {
                AffId = 1271808,
                AffName = "B2CAFF226",
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
                PreferredPayment = "Player",
                PlayerUsername = "bettingexp",
                NoSiteDescription = "test",
                NetellerAccountId = "1",
                MoneybookersEmail = "abc@gmail.com",
                BankName = "Test",
                BankAddress = "test",
                BankCode = "test",
                BankAccountName = "test",
                BankAccountNumber = "test",
                Question = "test",
                Answer = "test",
                MailOption = false
            };
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
                PreferredPayment = "Player",
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
