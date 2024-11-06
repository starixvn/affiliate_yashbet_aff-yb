using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.Services.Common;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ReadOnlySessionBaseController : BaseController
    {
        private readonly ISessionService _sessionService;

        protected ReadOnlySessionBaseController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        protected ISessionService SessionService => _sessionService;

        protected void AssignUserInfoParam(UserInfoParam param)
        {
            param.UserId = SessionService.UserId;
            param.Ip = Request.GetClientIP();
        }
    }
}
