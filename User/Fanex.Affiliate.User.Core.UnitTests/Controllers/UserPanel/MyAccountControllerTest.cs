using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class MyAccountControllerTest
    {
        private const string PlayerUserName = "player";
        private static MyAccountController CreateAccountController(
            ISessionService sessionService = null,
            IAccountRepository accountRepository = null,
            ILanguageService languageService = null,
            ICurrencyRepository currencyRepository = null)
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeAccountRepository = accountRepository ?? (new Mock<IAccountRepository>()).Object;
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeCurrencyRepository = currencyRepository ?? Mock.Of<ICurrencyRepository>();
            var controller = new MyAccountController(fakeSessionService, fakeLanguageService, fakeAccountRepository, fakeCurrencyRepository);
            return MockingHelper.FakeController(controller) as MyAccountController;
        }

        [Fact]
        public void GetMyAccount_WhenCall_ReturnCorrectModel()
        {
            // Arrange
            var expectedAccount = new AccountInformation { PreferredLanguage = Constant.DefaultLanguage };
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(ac => ac.GetAccountInfo(Constant.DefaultAffId)).Returns(expectedAccount);

            var languageService = new Mock<ILanguageService>();
            languageService.Setup(l => l.GetLanguageList(It.IsAny<bool?>())).Returns(new List<Language> { new Language { Code = "en-US", EnglishName = "English" } });
            var controller = CreateAccountController(null, accountRepository.Object, languageService.Object);

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as MyAccountViewModel;

            // Assert
            Assert.Equal(expectedAccount.PreferredLanguage, result.AccountInformation.PreferredLanguage);
            Assert.False(expectedAccount.IsRenderState);
            Assert.Equal("English", result.AccountInformation.LanguageName);
            Assert.Null(result.AccountInformation.ContactAppList);
        }

        [Fact]
        public void GetMyAccount_WhenCall_ReturnCorrectContactAppList()
        {
            // Arrange
            var contactApps = CreateContactAppList();
            var expectedResult = JsonConvert.SerializeObject(contactApps);
            var expectedAccount = new AccountInformation
            {
                PreferredLanguage = Constant.DefaultLanguage,
                ContactApps = expectedResult
            };
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(ac => ac.GetAccountInfo(Constant.DefaultAffId)).Returns(expectedAccount);
            var controller = CreateAccountController(null, accountRepository: accountRepository.Object);

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as MyAccountViewModel;

            // Assert
            Assert.Equal(expectedResult, result.AccountInformation.ContactApps);
            Assert.Equal(contactApps.Count, result.AccountInformation.ContactAppList.Count());
        }

        [Fact]
        public void GetMyAccount_ChineseLanguage_ReturnCorrectModel()
        {
            // Arrange
            const string ExpectedLanguageName = "aa-AA";
            var expectedAccount = new AccountInformation { PreferredLanguage = ExpectedLanguageName };
            var sessionService = MockingHelper.GetSessionService();
            sessionService.CurrentLanguage = "zh-TW";
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(ac => ac.GetAccountInfo(Constant.DefaultAffId)).Returns(expectedAccount);
            var controller = CreateAccountController(accountRepository: accountRepository.Object, sessionService: sessionService);

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as MyAccountViewModel;

            // Assert
            Assert.True(result.AccountInformation.IsRenderState);
            Assert.Empty(result.AccountInformation.MoreWebsiteUrlList);
            Assert.Equal(ExpectedLanguageName, result.AccountInformation.LanguageName);
        }

        [Fact]
        public void PostMyAccount_IsInternal_ReturnCorrectResult()
        {
            // Arrange
            var sessionService = MockingHelper.GetSessionService();
            sessionService.IsInternal = true;

            var controller = CreateAccountController(sessionService, null);

            // Action
            var actionResult = controller.Index(new AccountInformation()) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.False(result.IsOk);
            Assert.Equal(Lang.ErrAuthorized, result.Message);
        }

        [Fact]
        public void PostMyAccount_InvalidContactApp_ReturnCorrectResult()
        {
            // Arrange
            var controller = CreateAccountController(null, null);
            var contactAppList = CreateContactAppList();
            contactAppList.ForEach(item => item.Value = null);

            // Action
            var actionResult = controller.Index(new AccountInformation { ContactAppList = contactAppList }) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.False(result.IsOk);
            Assert.Equal(Error.RequireIMAccount, result.Message);
        }

        [Fact]
        public void PostMyAccount_ContactAppListHasAtLeastOneValue_CallGetAccountInfo()
        {
            // Arrange
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(ac => ac.GetAccountInfo(Constant.DefaultAffId)).Returns(new AccountInformation { PlayerUsername = PlayerUserName });
            accountRepository.Setup(ac => ac.UpdateAccountInformation(It.IsAny<UpdateAccountParam>())).Returns(new ResultInfo { IsOk = true });
            var controller = CreateAccountController(null, accountRepository.Object);
            var contactAppList = CreateContactAppList();
            contactAppList[0].Value = null;

            // Action
            controller.Index(new AccountInformation { ContactAppList = contactAppList });

            // Assert
            accountRepository.Verify(ac => ac.GetAccountInfo(Constant.DefaultAffId), Times.Once);
        }

        [Fact]
        public void PostMyAccount_UpdateFailed_ReturnCorrectResult()
        {
            // Arrange
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(ac => ac.UpdateAccountInformation(It.IsAny<UpdateAccountParam>())).Returns(new ResultInfo { IsOk = false });
            var controller = CreateAccountController(null, accountRepository.Object);

            // Action
            var actionResult = controller.Index(new AccountInformation { ContactAppList = CreateContactAppList() }) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.False(result.IsOk);
        }

        [Fact]
        public void PostMyAccount_UpdateSuccessfully_PasssedParamCorrectly()
        {
            // Arrange
            UpdateAccountParam passedParam = null;
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(ac => ac.GetAccountInfo(Constant.DefaultAffId)).Returns(new AccountInformation());
            accountRepository.Setup(ac => ac.UpdateAccountInformation(It.IsAny<UpdateAccountParam>()))
                .Returns(new ResultInfo { IsOk = true })
                .Callback<UpdateAccountParam>(p => passedParam = p);

            var expectedResult = JsonConvert.SerializeObject(CreateExpectedParam());
            var controller = CreateAccountController(null, accountRepository.Object);
            var requestParam = new AccountInformation
            {
                ContactAppList = CreateContactAppList(),
                PlayerUsername = PlayerUserName,
                Phone = "Phone",
                SiteUrl = "SiteUrl",
                MoreSiteUrls = "MoreSiteUrls",
                NoSiteDesc = "NoSiteDesc",
                AffId = Constant.DefaultAffId,
                PreferredPayment = PlayerUserName,
                TransferLimit = 0,
                PlayerCurrencyId = 0
            };

            // Action

            var actionResult = controller.Index(requestParam) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.True(result.IsOk);
            Assert.True(requestParam.IsPlayerAccount);
            Assert.Single(requestParam.MoreWebsiteUrlList);
            Assert.Equal(expectedResult, JsonConvert.SerializeObject(passedParam));
        }

        private UpdateAccountParam CreateExpectedParam()
        {
            var appContactList = CreateContactAppList().Select(p => new ContactApp
            {
                Name = $"Encoded{p.Name}",
                Value = $"Encoded{p.Value}",
                Order = p.Order
            });

            return new UpdateAccountParam
            {
                Phone = "EncodedPhone",
                SiteUrl = "EncodedSiteUrl",
                MoreSiteUrls = "EncodedMoreSiteUrls",
                NoSiteDesc = "EncodedNoSiteDesc",
                AffId = Constant.DefaultAffId,
                Ip = "::1",
                Language = Constant.DefaultLanguage,
                ContactApps = JsonConvert.SerializeObject(appContactList)
            };
        }

        private static List<ContactApp> CreateContactAppList()
        {
            return new List<ContactApp> {
                    new ContactApp
                    {
                        Name = "Skype",
                        Value = "skyper",
                        Order = 0
                    },
                    new ContactApp
                    {
                        Name = "Zalo",
                        Value = "Zalor",
                        Order = 1
                    }
                };
        }
    }
}