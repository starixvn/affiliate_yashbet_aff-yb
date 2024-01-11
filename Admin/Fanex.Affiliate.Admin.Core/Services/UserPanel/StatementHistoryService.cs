using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public class StatementHistoryService : IStatementHistoryService
    {
        private readonly IStatementHistoryRepository _statementHistoryRepository;

        public StatementHistoryService(IStatementHistoryRepository statementHistoryRepository)
        {
            _statementHistoryRepository = statementHistoryRepository;
        }

        public EntityResult<SubAffiliate> GetSubAffiliate(SubAffiliateParam param)
        {
            return _statementHistoryRepository.GetSubAffiliate(param);
        }

        public DailyEarningResult GetDailyEarning(DailyEarningParam param)
        {
            return _statementHistoryRepository.GetDailyEarning(param);
        }

        public DailyEarningDetailResult GetDailyEarningDetail(DailyEarningParam param)
        {
            var rawData = _statementHistoryRepository.GetDailyEarningDetail(param);

            var total = new DailyEarningDetail();

            foreach (var item in rawData)
            {
                total.Ggr += item.Ggr;
                total.Deposit += item.Deposit;
                total.Withdrawal += item.Withdrawal;
                total.Bonus += item.Bonus;
                total.Rebate += item.Rebate;
            }

            return new DailyEarningDetailResult
            {
                MainData = rawData,
                Total = total
            };
        }

        public EntityResult<StatementHistory> GetStatementHistoryList(AffiliatePagingByDateParam param)
        {
            var vms = new List<StatementHistory>();
            var result = _statementHistoryRepository.GetStatementHistoryList(param);
            foreach (var item in result.MainData)
            {
                if (item.TransType == TransType.MonthlyEarning || item.TransType == TransType.SubAffiliate)
                {
                    if (CheckIfExistMonthlyTotal(item.Date, vms))
                    {
                        continue;
                    }
                    vms.Add(CreateMonthlyTotal(item.Date, result.MainData));
                }
                else
                {
                    vms.Add(item);
                }
            }

            return new EntityResult<StatementHistory>
            {
                MainData = vms,
                TotalItem = result.TotalItem
            };
        }

        private static StatementHistory CreateMonthlyTotal(DateTime monthDate, IEnumerable<StatementHistory> statementHistoryList)
        {
            var children = statementHistoryList.Where(s => s.Date == monthDate)
                .Where(s => s.TransType == TransType.SubAffiliate || s.TransType == TransType.MonthlyEarning);

            var monthlyCom = children.FirstOrDefault(s => s.TransType == TransType.MonthlyEarning);

            var monthlyTotal = new StatementHistory
            {
                Creator = monthlyCom?.Creator ?? children.FirstOrDefault().Creator,
                Amount = children.Sum(s => s.Amount),
                Date = monthDate,
                Children = children,
                Status = monthlyCom?.Status ?? children.FirstOrDefault().Status,
                TransType = TransType.MonthlyTotal,
                CalculatedType = monthlyCom?.CalculatedType ?? children.FirstOrDefault().CalculatedType
            };
            return monthlyTotal;
        }

        private static bool CheckIfExistMonthlyTotal(DateTime monthDate, List<StatementHistory> vms)
        {
            return vms.Exists(m => m.Date == monthDate && m.TransType == TransType.MonthlyTotal);
        }
    }
}