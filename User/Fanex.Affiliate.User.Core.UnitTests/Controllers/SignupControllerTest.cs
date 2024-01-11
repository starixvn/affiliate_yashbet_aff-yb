using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class SignupControllerTest
    {
        [Fact]
        public void Index_WhenCallWithGet_ReturnView()
        {
            // Arrange
            var controller = CreateController();

            // Action
            var result = controller.Index() as ViewResult;
            var model = result.Model as UserSignUpParam;

            // Assert
            Assert.NotEmpty(result.ViewName);
            Assert.True(model.IsValidParentCode);
        }

        [Fact]
        public void Index_WhenCallWithRefIdNotNull_ReturnView()
        {
            // Arrange
            var accountService = new Mock<IAccountService>();
            accountService.Setup(a => a.GetGeneralAffInfo(123, null)).Returns(new GeneralAffInfo { AffId = 123, AffCode = "12345" });
            var controller = CreateController(accountService.Object);

            // Action
            var result = controller.Index("123", "abc") as ViewResult;
            var model = result.Model as UserSignUpParam;

            // Assert
            Assert.NotEmpty(result.ViewName);
            Assert.True(model.IsValidParentCode);
        }

        [Fact]
        public void Index_WhenCallWithAffIdIdNotNullAndAffCodeIsNull_ReturnView()
        {
            // Arrange
            var accountService = new Mock<IAccountService>();
            accountService.Setup(a => a.GetGeneralAffInfo(null, "12345")).Returns(new GeneralAffInfo { AffId = 123, AffCode = "12345" });
            var controller = CreateController(accountService.Object);

            // Action
            var result = controller.Index("abc", null, "12345") as ViewResult;
            var model = result.Model as UserSignUpParam;

            // Assert
            Assert.NotEmpty(result.ViewName);
            Assert.True(model.IsValidParentCode);
        }

        [Fact]
        public void Index_WhenCallWithInvalidParam_ReturnView()
        {
            // Arrange
            var controller = CreateController();

            // Action
            var result = controller.Index("index", "abc", "abc") as ViewResult;
            var model = result.Model as UserSignUpParam;

            // Assert
            Assert.NotEmpty(result.ViewName);
            Assert.False(model.IsValidParentCode);
        }

        [Fact]
        public void SecurityQuestion_WhenCall_ReturnView()
        {
            // Arrange
            var controller = CreateController();

            // Action
            var result = controller.SecurityQuestion() as PartialViewResult;

            // Assert
            Assert.Equal("~/Views/Signup/_SecurityQuestion.cshtml", result.ViewName);
        }

        [Fact]
        public async Task Index_ModelStateIsInvalid_ReturnCorrectResult()
        {
            // Arrange
            const string ErrorMessage1 = "ErrorMessage1";
            const string ErrorMessage2 = "ErrorMessage2";
            var accountService = new Mock<IAccountService>();
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("PRO");
            var controller = CreateController(accountService.Object, appSetting: appSetting.Object);
            controller.ModelState.AddModelError("Error1", ErrorMessage1);
            controller.ModelState.AddModelError("Error2", ErrorMessage2);

            var contactAppList = CreateAppContactList();
            var expectedResult = new ResultInfo { IsOk = false, Message = $"{ErrorMessage1}<br>{ErrorMessage2}" };

            // Action
            var actionResult = await controller.Index(new UserSignUpParam
            {
                Captcha = Constant.Captcha,
                ContactAppList = contactAppList
            }) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            accountService.Verify(p => p.CreateAffiliateAsync(It.IsAny<UserSignUpParam>(), It.IsAny<EmailBaseParam>()), Times.Never);
            Assert.Equal(expectedResult.IsOk, result.IsOk);
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task Index_ContactAppListIsInvalid_ReturnCorrectResult()
        {
            // Arrange
            var accountService = new Mock<IAccountService>();
            var appSetting = new Mock<IAppSetting>();
            appSetting.Setup(a => a.Environment).Returns("PRO");
            var controller = CreateController(accountService.Object, appSetting: appSetting.Object);
            var contactAppList = CreateAppContactList();
            contactAppList.ForEach(item => item.Value = null);

            // Action
            var actionResult = await controller.Index(new UserSignUpParam { Captcha = Constant.Captcha, ContactAppList = contactAppList }) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            accountService.Verify(p => p.CreateAffiliateAsync(It.IsAny<UserSignUpParam>(), It.IsAny<EmailBaseParam>()), Times.Never);
            Assert.False(result.IsOk);
            Assert.Equal(Error.RequireIMAccount, result.Message);
        }

        [Fact]
        public async Task Index_WhenCallWithPost_CaptchaIsInvalid_SetResultCorrectly()
        {
            // Arrange
            var captchaService = new Mock<ICaptchaService>();
            captchaService.Setup(c => c.IsValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var controller = CreateController(captchaService: captchaService.Object);
            controller.HttpContext.Request.Cookies.Add(new HttpCookie("Fanex.UserSite.RefCode", "12345"));

            var signUpParam = new UserSignUpParam { Captcha = "Invalid" };

            var expectedResult = new ResultInfo { IsOk = false, Message = Lang.ValidationCode };

            // Action
            var actionResult = await controller.Index(signUpParam) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.Equal(expectedResult.IsOk, result.IsOk);
            Assert.Equal(expectedResult.Message, result.Message);
        }

        [Fact]
        public async Task Index_WhenCallWithPost_ModelStateIsValid_ReturnResultCorrectly()
        {
            // Arrange
            var expectedResult = new ResultInfo { IsOk = true };
            var accountService = new Mock<IAccountService>();
            accountService.Setup(p => p.CreateAffiliateAsync(It.IsAny<UserSignUpParam>(), It.IsAny<EmailBaseParam>())).Returns(Task.FromResult(expectedResult));

            var appSetting = Mock.Of<IAppSetting>(ap => ap.AllowedAffiliateTypes == new List<string> { Constant.AffTypeAsian }
                && ap.Environment == "LOCAL");

            var controller = CreateController(accountService.Object, appSetting: appSetting);
            controller.TempData = new TempDataDictionary();

            var signUpParam = CreateParam();
            signUpParam.Captcha = Constant.Captcha;

            // Action
            var actionResult = await controller.Index(signUpParam) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        private static SignupController CreateController(
            IAccountService accountService = null,
            ISessionService sessionService = null,
            IAppSetting appSetting = null,
            ICaptchaService captchaService = null,
            ILanguageService languageService = null,
            ISecurityQuestionService securityQuestionService = null)
        {
            sessionService = sessionService ?? MockingHelper.GetSessionService();
            sessionService.SignUpCaptchaCode = Constant.Captcha;
            accountService = accountService ?? Mock.Of<IAccountService>();
            appSetting = appSetting ?? Mock.Of<IAppSetting>();
            captchaService = captchaService ?? CreateDefaultCaptchaService();
            languageService = languageService ?? Mock.Of<ILanguageService>();
            securityQuestionService = securityQuestionService ?? Mock.Of<ISecurityQuestionService>();

            var controller = new SignupController(sessionService, appSetting, accountService, languageService, captchaService, securityQuestionService);
            var request = MockingHelper.GetHttpRequest();
            var response = new Mock<HttpResponseBase>();
            response.Setup(p => p.Cookies).Returns(new HttpCookieCollection());

            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.HtmlEncode(It.IsAny<string>())).Returns<string>(s => $"Encoded{s}");

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext.Request == request &&
                ctx.HttpContext.Server == server.Object &&
                ctx.HttpContext.Response == response.Object
            );

            var context = Mock.Of<HttpContextBase>(ctx => ctx.Request == request);
            var requestContext = new RequestContext(context, new RouteData());
            controller.Url = new UrlHelper(requestContext);

            return controller;
        }

        private static ICaptchaService CreateDefaultCaptchaService()
        {
            var mockCaptchaService = new Mock<ICaptchaService>();
            mockCaptchaService.Setup(p => p.IsValid(Constant.Captcha, Constant.Captcha, It.IsAny<string>()))
                .Returns(true);

            return mockCaptchaService.Object;
        }

        private UserSignUpParam CreateParam()
        {
            return new UserSignUpParam
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone",
                Address = "Address",
                City = "City",
                Postal = "Postal",
                Country = "Country",
                SiteUrl = "SiteUrl",
                MoreSiteUrls = "MoreSiteUrls",
                NoSiteDesc = "NoSiteDesc",
                PreferredLanguage = "PreferredLanguage",
                QuestionId = 1,
                Answer = "Answer",
                Player_UserName = "Player_UserName",
                Gender = true,
                State = "State",
                ContactApps = "ContactApps",
                ContactAppList = CreateAppContactList()
            };
        }

        private static List<ContactApp> CreateAppContactList()
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