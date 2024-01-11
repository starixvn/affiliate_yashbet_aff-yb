using Affiliate.YB.DTO;
using Affiliate.YB.Models;
using Affiliate.YB.SpParams.Commands;
using Affiliate.YB.SpParams.Criteria;
using Affiliate.YB.WebApi.DTO;
using Affiliate.YB.WebApi.Models;
using Fanex.Data.Repository;
using Fanex.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Affiliate.YB.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<AffCustomerModel>> GetAffCustomers(DateTime fromDate)
        {
            var customerList = await dynamicRepository.FetchAsync<AffCustomerInfo>(new GetAffCustomersCriteria(fromDate));

            return customerList.Select(c => new AffCustomerModel
            {
                ActStatus = c.ActStatus,
                SiteName = c.SiteName,
                CurrencyId = c.CurrencyId,
                CustId = c.CustId,
                RegTime = c.RegTime,
                SiteId = c.SiteId,
                Status = c.Status,
                UserName = c.UserName,
                AffId = c.AffId,
                FirstDepositDate = c.FirstDepositTime,
                IsTest = c.IsTest,
                AffCode = c.AffCode
            });
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerInfo>> GetCustomers(DateTime fromDate, int batchCount = 2000)
            => await dynamicRepository.FetchAsync<CustomerInfo>(new GetCustomersCriteria(fromDate, batchCount));

        [HttpGet]
        public async Task<IEnumerable<CustomerInfo>> GetFixedCustomers()
            => await dynamicRepository.FetchAsync<CustomerInfo>(new GetFixedCustomerCriteria());

        [HttpGet]
        public async Task<IEnumerable<CustWalletInfo>> GetCustWallets(DateTime fromDate)
            => await dynamicRepository.FetchAsync<CustWalletInfo>(new GetCustWalletCriteria(fromDate));

        [HttpGet]
        public async Task<CustomerInfo> GetLoginCustomerInfo(string userName, int siteId)
        {
            CustomerInfo cust = null;
            if (!HasNonASCIIChars(userName))
            {
                cust = await dynamicRepository.GetAsync<CustomerInfo>(new GetLoginCustInfoCriteria() { UserName = userName, SiteId = siteId });
            }
            return cust;
        }

        [HttpGet]
        public async Task<IEnumerable<CustRegModel>> GetCustomerRegInfo(long unixTime)
        {
            var infos = await dynamicRepository.FetchAsync<CustRegInfo>(new GetCustRegInfoCriteria(unixTime));
            return infos.Select(i => new CustRegModel
            {
                CustId = i.CustId,
                RegTime = DateTimeOffset.Parse(i.RegTime),
                SignUpUrl = i.SignUpUrl,
                SiteId = i.SiteId
            });
        }

        [HttpPost]
        public async Task<decimal> UpdateCustomerWallet(PaymentRequestInfo payment)
        {
            await LogService.InfoAsync($"Recevice update wallet request\n" +
                $"Param: {JsonConvert.SerializeObject(payment)}\n" +
                $"RequestIp: ${GetIpAddress()}");
            var criteria = new UpdateCustomerWalletCommand
            {
                Amount = payment.Amount,
                AfterAmount = 0,
                CustId = payment.CustId,
                Memo = payment.Memo,
                RefTransId = payment.RefTransId,
                SiteId = payment.SiteId,
                Type = payment.Type,
                UserName = payment.UserName
            };

            await dynamicRepository.ExecuteAsync(criteria);
            await LogService.InfoAsync($"AfterAmount: {criteria.AfterAmount}");
            return criteria.AfterAmount;
        }

        [HttpGet]
        public async Task<IEnumerable<CustBonusRolloverModel>> GetCustBonusRollOvers(int id, int batchCount = 2000)
            => await dynamicRepository.FetchAsync<CustBonusRolloverModel>(new GetCustBonusRolloverCriteria(id, batchCount));

        [HttpGet]
        public async Task<IEnumerable<CustClosingBalanceModel>> GetCustClosingBalance(DateTime lastGetTime, int pageIndex, int pageSize = 2000)
            => await dynamicRepository.FetchAsync<CustClosingBalanceModel>(new GetCustClosingBalanceCriteria(lastGetTime, pageIndex, pageSize));

        [HttpPost]
        public async Task<ResultInfo> UnlockUser(int[] userIds)
        {
            try
            {
                var json = BuildJson(userIds);
                await dynamicRepository.ExecuteAsync(new UnlockUsersCommand { CustIdList = json });
                return new ResultInfo
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ResultInfo
                {
                    IsSuccess = false,
                    ErrMsg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<ResultInfo> InsertAffiliateInfo(AffiliateInfo affiliate)
        {
            try
            {
                await dynamicRepository.ExecuteAsync(new InsertAffiliateInfoCommand
                {
                    AffiliateCode = affiliate.AffiliateCode,
                    AffiliateName = affiliate.AffiliateName,
                    CustId = affiliate.CustId,
                    ParentAffiliate = affiliate.ParentAffiliate,
                    SiteId = affiliate.SiteId,
                    UserName = affiliate.UserName
                });
                return new ResultInfo { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResultInfo
                {
                    IsSuccess = false,
                    ErrMsg = ex.Message
                };
            }
        }

        [HttpGet]
        public async Task<IEnumerable<CustBalanceInfo>> GetCustBalance([FromUri]int[] ids)
        {
            var json = BuildJson(ids);
            return await dynamicRepository.FetchAsync<CustBalanceInfo>(new GetCustBalanceCriteria(json));
        }

        [HttpGet]
        public async Task<IEnumerable<VipUpgradeReviewInfo>> GetVipUpgradeReviews(DateTime monthDate, int pageIndex, int pageSize)
        {
            var txtMonthDate = monthDate.ToString("yyyyMM");
            return await dynamicRepository.FetchAsync<VipUpgradeReviewInfo>(new GetVipUpgradeReviewCriteria
            {
                MonthDate = txtMonthDate,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
        }

        [HttpPost]
        public async Task<ResultInfo> TransferMoneyToPlayer(TransferMoneyParam param)
        {
            await LogService.InfoAsync($"Receive transfer money request\n" +
                $"Param: {JsonConvert.SerializeObject(param)}\n" +
                $"RequestIp: ${GetIpAddress()}");

            var command = new TransferMoneyToPlayerCommand
            {
                SiteId = param.SiteId,
                ErrCode = 0,
                FromAmount = param.FromAmount,
                FromCustId = param.FromCustId,
                MaxTransferAmount = param.MaxTransferAmount,
                RefTransId = param.RefTransId,
                ToAmount = param.ToAmount,
                ToCustId = param.ToCustId,
            };
            try
            {
                await dynamicRepository.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                await LogService.ErrorAsync($"{ex.Message}");
                throw;
            }

            await LogService.InfoAsync($"ErrCode: {command.ErrCode}");
            return new ResultInfo
            {
                ErrMsg = command.ErrCode.ToString(),
                IsSuccess = command.ErrCode == 0
            };
        }

        [HttpGet]
        public async Task<IEnumerable<CustVerificationReward>> GetCustVerificationRewards(DateTime lastGetTime, string conditionField, int batchCount = 2000)
            => await dynamicRepository.FetchAsync<CustVerificationReward>(new GetCustVerificationRewardCriteria(lastGetTime, conditionField, batchCount));

        private string BuildJson(int[] userIds)
            => $"[{String.Join(",", userIds)}]";

        private string GetIpAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private bool HasNonASCIIChars(string str)
            => Encoding.UTF8.GetByteCount(str) != str.Length;
    }
}