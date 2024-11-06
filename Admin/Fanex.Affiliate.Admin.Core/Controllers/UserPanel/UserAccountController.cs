using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.ViewModels.Customer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.UserPanel
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class UserAccountController : ReadOnlySessionBaseController
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(
            ISessionService sessionService,
            IUserAccountService userAccountService) : base(sessionService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [UnauthorizedPropertyCensor]
        public ActionResult GetUserAccountInfo(int affId)
        {
            if (affId >= Validation.MinId)
            {
                var userAccountInfo = _userAccountService.GetUserAccountInfo(affId);

                if (userAccountInfo != null)
                {
                    var newUserAccountInfo = new UserAccount
                    {
                        AffId = userAccountInfo.AffId,
                        AffName = userAccountInfo.AffName,
                        FirstName = Server.HtmlDecode(userAccountInfo.FirstName),
                        LastName = Server.HtmlDecode(userAccountInfo.LastName),
                        Gender = userAccountInfo.Gender,
                        Email = Server.HtmlDecode(userAccountInfo.Email),
                        Phone = Server.HtmlDecode(userAccountInfo.Phone),
                        ContactApps = Server.HtmlDecode(userAccountInfo.ContactApps),
                        Address = Server.HtmlDecode(userAccountInfo.Address),
                        City = Server.HtmlDecode(userAccountInfo.City),
                        State = Server.HtmlDecode(userAccountInfo.State),
                        Postcode = Server.HtmlDecode(userAccountInfo.Postcode),
                        Country = Server.HtmlDecode(userAccountInfo.Country),
                        SiteUrl = Server.HtmlDecode(userAccountInfo.SiteUrl),
                        MoreSiteUrls = Server.HtmlDecode(userAccountInfo.MoreSiteUrls),
                        NoSiteDescription = Server.HtmlDecode(userAccountInfo.NoSiteDescription),
                        PreferredPayment = Server.HtmlDecode(userAccountInfo.PreferredPayment),
                        PlayerUsername = Server.HtmlDecode(userAccountInfo.PlayerUsername),
                        NetellerAccountId = Server.HtmlDecode(userAccountInfo.NetellerAccountId),
                        MoneybookersEmail = Server.HtmlDecode(userAccountInfo.MoneybookersEmail),
                        BankName = Server.HtmlDecode(userAccountInfo.BankName),
                        BankAddress = Server.HtmlDecode(userAccountInfo.BankAddress),
                        BankCode = Server.HtmlDecode(userAccountInfo.BankCode),
                        BankAccountName = Server.HtmlDecode(userAccountInfo.BankAccountName),
                        BankAccountNumber = Server.HtmlDecode(userAccountInfo.BankAccountNumber),
                        Question = Server.HtmlDecode(userAccountInfo.Question),
                        Answer = Server.HtmlDecode(userAccountInfo.Answer),
                        MailOption = userAccountInfo.MailOption
                    };

                    if (!string.IsNullOrEmpty(newUserAccountInfo.ContactApps))
                    {
                        newUserAccountInfo.ContactAppList = JsonConvert.DeserializeObject<IEnumerable<ContactApp>>(newUserAccountInfo.ContactApps).OrderBy(p => p.Order);
                    }

                    return GenerateJsonNetResult(newUserAccountInfo);
                }
            }

            return new JsonNetResult();
        }

        [FunctionAuthorize(FunctionName = Function.EditAffDetails, IsHost = true)]
        [HttpPost]
        [ValidateAntiForgeryHeader]
        public async Task<ActionResult> UpdateUserAccount(UserAccountParam param)
        {
            if (ModelState.IsValid)
            {
                var userAccountParam = new UserAccountParam
                {
                    AffId = param.AffId,
                    FirstName = Server.HtmlEncode(param.FirstName),
                    LastName = Server.HtmlEncode(param.LastName),
                    Gender = param.Gender,
                    Email = Server.HtmlEncode(param.Email),
                    Phone = Server.HtmlEncode(param.Phone),
                    Address = Server.HtmlEncode(param.Address),
                    City = Server.HtmlEncode(param.City),
                    State = Server.HtmlEncode(param.State),
                    Postcode = Server.HtmlEncode(param.Postcode),
                    Country = Server.HtmlEncode(param.Country),
                    SiteUrl = Server.HtmlEncode(param.SiteUrl),
                    MoreSiteUrls = Server.HtmlEncode(param.MoreSiteUrls),
                    NoSiteDescription = Server.HtmlEncode(param.NoSiteDescription),
                    PreferredPayment = Server.HtmlEncode(param.PreferredPayment),
                    PlayerUsername = Server.HtmlEncode(param.PlayerUsername),
                    NetellerAccountId = Server.HtmlEncode(param.NetellerAccountId),
                    MoneybookersEmail = Server.HtmlEncode(param.MoneybookersEmail),
                    BankName = Server.HtmlEncode(param.BankName),
                    BankAddress = Server.HtmlEncode(param.BankAddress),
                    BankCode = Server.HtmlEncode(param.BankCode),
                    BankAccountName = Server.HtmlEncode(param.BankAccountName),
                    BankAccountNumber = Server.HtmlEncode(param.BankAccountNumber),
                    MailOption = param.MailOption,
                    HasWebSite = param.HasWebSite,
                };
                AssignUserInfoParam(userAccountParam);

                if (param.ContactAppList != null)
                {
                    userAccountParam.ContactApps = JsonConvert.SerializeObject(param.ContactAppList.Select(p => new ContactApp
                    {
                        Name = Server.HtmlEncode(p.Name),
                        Value = Server.HtmlEncode(p.Value),
                        Order = p.Order
                    }));
                }

                return GenerateJsonNetResult(await _userAccountService.UpdateUserAccountAsync(userAccountParam));
            }

            return GenerateModelStateErrResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetPaymentMethod()
        {
            var paymentInfo = _userAccountService.GetPaymentMethod();
            var paymentMethods = paymentInfo.paymentMethods
                .Select(m => new KeyValuePair<string, string>(m.PaymentId, m.PaymentName));
            var vms = new PaymentMethodViewModel
            {
                MinAmount = paymentInfo.minAmount,
                PaymentMethods = paymentMethods
            };

            var result = new JsonNetResult
            {
                Data = vms
            };
            return result;
        }
    }
}
