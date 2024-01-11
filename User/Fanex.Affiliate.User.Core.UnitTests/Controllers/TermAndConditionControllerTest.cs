using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using NSubstitute;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class TermAndConditionControllerTest
    {
        private readonly IAppSetting AppSetting;
        private readonly ISessionService SessionService;
        private readonly ILanguageService LanguageService;

        public TermAndConditionControllerTest()
        {
            AppSetting = Substitute.For<IAppSetting>();
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
        }

        [Fact]
        public void Index_WhenCall_ReturnCorrectView()
        {
            var controller = CreateController();

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Contains("Views/TermAndCondition/Index.cshtml", result.ViewName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("darkmode")]
        public void Popup_WhenCall_ReturnCorrectView(string mode)
        {
            var controller = CreateController();

            var result = controller.Popup() as ViewResult;

            Assert.NotNull(result);
            Assert.Contains("Views/TermAndCondition/Popup.cshtml", result.ViewName);
        }

        private TermAndConditionController CreateController()
            => new TermAndConditionController(AppSetting, SessionService, LanguageService);
    }
}
