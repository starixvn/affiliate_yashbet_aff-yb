using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class MainControllerTest
    {
        private readonly ISessionService SessionService;
        private readonly ILanguageService LanguageService;

        public MainControllerTest()
        {
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
        }

        [Fact]
        public void Index_WhenCall_ReturnCorrecView()
        {
            var controller = new MainController(SessionService, LanguageService);
            var result = controller.Index() as ViewResult;

            Assert.Contains("Main/Index.cshtml", result.ViewName);
        }
    }
}
