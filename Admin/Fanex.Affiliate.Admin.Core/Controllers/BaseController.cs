using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Extensions;
using ONEbook.UM.AspNetMVC.Filters;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [CustomAuthorize]
    [CheckUMFilter]
    public class BaseController : Controller
    {
        protected ActionResult GenerateModelStateErrResult() => new JsonNetResult
        {
            Data = ResultInfo.GenerateErr(string.Join("<br />", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)))
        };

        protected ActionResult GenerateErrResult(string messageErr = "") => new JsonNetResult
        {
            Data = ResultInfo.GenerateErr(messageErr)
        };

        protected ActionResult GenerateJsonNetResult(object data) => new JsonNetResult
        {
            Data = data
        };
    }
}
