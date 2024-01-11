using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Customer;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class CustomerStatisticsController : ReadOnlySessionBaseController
    {
        private readonly ICustomerStatisticsService _customerStatisticsService;

        public CustomerStatisticsController(
            ISessionService sessionService,
            ICustomerStatisticsService customerStatisticsService) : base(sessionService)
        {
            _customerStatisticsService = customerStatisticsService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetCustomerStatistics(CustomerStatisticsParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_customerStatisticsService.GetCustomerStatistics(param));
            }

            return new JsonNetResult();
        }
    }
}