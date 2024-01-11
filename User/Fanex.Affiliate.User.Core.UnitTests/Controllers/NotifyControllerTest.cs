using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Newtonsoft.Json;
using NSubstitute;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class NotifyControllerTest
    {
        private readonly IAppSetting AppSetting;
        private readonly ISessionService SessionService;
        private readonly ILanguageService LanguageService;

        public NotifyControllerTest()
        {
            AppSetting = Substitute.For<IAppSetting>();
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
        }

        [Fact]
        public void Error_WhenCall_ReturnCorrectView()
        {
            var controller = CreateController();

            var result = controller.Error() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void PageNotFound_WhenCall_ReturnCorrectView()
        {
            var controller = CreateController();

            var result = controller.PageNotFound() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void CodeExpired_WhenCall_ReturnIsNotOkResult()
        {
            var controller = CreateController();

            var result = controller.CodeExpired() as JsonResult;

            var returnData = new
            {
                IsOk = false,
                IsExpired = true
            };

            Assert.Equal(JsonConvert.SerializeObject(returnData), JsonConvert.SerializeObject(result.Data));
        }

        private NotifyController CreateController()
            => new NotifyController(AppSetting, SessionService, LanguageService);
    }
}
