using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;

namespace Fanex.Affiliate.User.Core.Controllers
{
    public class BaseLayoutController : BaseController
    {
        protected readonly IAppSetting AppSetting;

        public BaseLayoutController(
            IAppSetting appSetting,
            ISessionService sessionService,
            ILanguageService languageService) : base(sessionService, languageService)
        {
            AppSetting = appSetting;
        }

        protected BaseLayoutViewModel GetBaseLayoutViewModel()
        {
            return new BaseLayoutViewModel
            {
                SiteName = AppSetting.SiteName,
                SiteFriendlyName = AppSetting.SiteFriendlyName,
                CurrentLanguage = SessionService.CurrentLanguage
            };
        }

        protected void AssignBaseLayoutViewModel(BaseLayoutViewModel model)
        {
            model.SiteName = AppSetting.SiteName;
            model.SiteFriendlyName = AppSetting.SiteFriendlyName;
            model.CurrentLanguage = SessionService.CurrentLanguage;
        }
    }
}
