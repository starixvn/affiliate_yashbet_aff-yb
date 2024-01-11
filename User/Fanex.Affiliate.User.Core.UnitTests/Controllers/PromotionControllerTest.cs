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
    public class PromotionControllerTest
    {
        private readonly IAppSetting AppSetting;
        private readonly ISessionService SessionService;
        private readonly ILanguageService LanguageService;

        public PromotionControllerTest()
        {
            AppSetting = Substitute.For<IAppSetting>();
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
        }

        [Fact]
        public void Index_WhenCall_ReturnCorrectView()
        {
            var controller = new PromotionController(AppSetting, SessionService, LanguageService);
            NSubtituteMockingHelper.FakeController(controller);

            var result = controller.Index() as ViewResult;

            Assert.Contains("~/Views/Promotion/Index.cshtml", result.ViewName);
        }
    }
}
