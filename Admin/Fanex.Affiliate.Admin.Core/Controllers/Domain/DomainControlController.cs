using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Domain;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Affiliate.Admin.Core.ViewModels.Domain;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.Domain
{
    [FunctionAuthorize(FunctionName = Function.Domain)]
    public class DomainControlController : ReadOnlySessionBaseController
    {
        private readonly IDomainControlService _domainControlService;
        private readonly ILanguageRepository _languageRepository;

        public DomainControlController(
            ISessionService sessionService,
            IDomainControlService domainControlService,
            ILanguageRepository languageRepository) : base(sessionService)
        {
            _domainControlService = domainControlService;
            _languageRepository = languageRepository;
        }

        public ActionResult Index()
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.AffiliateDomain, Url = Url.Content("~/Portal/Domain?isAffDomain=true") },
                new SubMenu { Name = SiteFriendlyNameHelper.ReplaceB2C(Lang.TextB2CDomains), Url = Url.Content("~/Portal/Domain?isAffDomain=false") },
                new SubMenu { Name = Lang.DomainControl, Url = Url.Content("~/Portal/DomainControl"), IsActive = true },
                new SubMenu { Name = Lang.SubAffiliateDomain, Url = Url.Content("~/Portal/SubAffDomain") }
            };

            var model = new DomainControlViewModel
            {
                SubMenuList = subMenu,
                MarketList = _languageRepository.GetLanguageList()
            };

            return View("~/Areas/Portal/Views/Domain/DomainControl.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetDomainControlList(string market)
        {
            return GenerateJsonNetResult(_domainControlService.GetDomainControlList(market));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Domain, IsHost = true)]
        public ActionResult InsertDomainControl(InsertUpdateDomainControlParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_domainControlService.InsertDomainControl(param));
            }

            return GenerateErrResult("Domain's info is invalid!");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Domain, IsHost = true)]
        public ActionResult UpdateDomainControl(InsertUpdateDomainControlParam param)
        {
            var isValidDomainId = param.DomainId >= Validation.MinId;
            if (ModelState.IsValid && isValidDomainId)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_domainControlService.UpdateDomainControl(param));
            }

            return GenerateErrResult(isValidDomainId ? "Domain's info is invalid!" : "Domain Id is invalid.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Domain, IsHost = true)]
        public ActionResult DeleteDomainControl(DeleteDomainControlParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_domainControlService.DeleteDomainControl(param));
            }

            return GenerateErrResult("Domain Id is invalid.");
        }
    }
}
