using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
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
    public class DomainController : ReadOnlySessionBaseController
    {
        private readonly IDomainService _domainService;

        public DomainController(
            ISessionService sessionService,
            IDomainService domainService) : base(sessionService)
        {
            _domainService = domainService;
        }

        public ActionResult Index(bool isAffDomain = true)
        {
            var subMenu = new List<SubMenu>
            {
                new SubMenu { Name = Lang.AffiliateDomain, Url = Url.Content("~/Portal/Domain?isAffDomain=true"), IsActive = isAffDomain },
                new SubMenu { Name = SiteFriendlyNameHelper.ReplaceB2C(Lang.TextB2CDomains), Url = Url.Content("~/Portal/Domain?isAffDomain=false"), IsActive = !isAffDomain },
                new SubMenu { Name = Lang.DomainControl, Url = Url.Content("~/Portal/DomainControl") },
                new SubMenu { Name = Lang.SubAffiliateDomain, Url = Url.Content("~/Portal/SubAffDomain") }
            };

            var model = new DomainViewModel
            {
                SubMenuList = subMenu,
                IsAffDomain = isAffDomain
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetDomainList(bool isAffDomain)
        {
            return GenerateJsonNetResult(_domainService.GetDomainList(isAffDomain));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Domain, IsHost = true)]
        public ActionResult InsertDomain(InsertUpdateDomainParam param)
        {
            if (ModelState.IsValid)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_domainService.InsertDomain(encodeParam));
            }

            return GenerateErrResult("Please input Domain's name.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Domain, IsHost = true)]
        public ActionResult UpdateDomain(InsertUpdateDomainParam param)
        {
            var isValidDomainId = param.DomainId >= Validation.MinId;
            if (ModelState.IsValid && isValidDomainId)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_domainService.UpdateDomain(encodeParam));
            }

            return GenerateErrResult(isValidDomainId ? "Please input Domain's name." : "Domain Id is invalid.");
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Domain, IsHost = true)]
        public ActionResult DeleteDomain(DeleteDomainParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_domainService.DeleteDomain(param));
            }

            return GenerateErrResult("Domain id is incorrect.");
        }

        private InsertUpdateDomainParam EncodeParam(InsertUpdateDomainParam param)
        {
            return new InsertUpdateDomainParam
            {
                DomainId = param.DomainId,
                DomainName = Server.HtmlEncode(param.DomainName),
                IsSSL = param.IsSSL,
                IsAffDomain = param.IsAffDomain
            };
        }
    }
}
