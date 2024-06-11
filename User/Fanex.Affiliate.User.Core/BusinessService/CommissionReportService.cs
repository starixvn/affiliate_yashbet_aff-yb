using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using System.Linq;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class CommissionReportService : ICommissionReportService
    {
        private const int PercentageUnit = 100;

        private readonly ICommissionReportRepository _commissionReportRepository;

        public CommissionReportService(ICommissionReportRepository commissionReportRepository)
        {
            _commissionReportRepository = commissionReportRepository;
        }

        private static string GetCalculatedNote(int? type)
        {
            if (!type.HasValue || type == CommissionType.Calculated)
            {
                return string.Empty;
            }
            return type.Value == 0 ? $" ({Lang.Temporary})" : $" ({Lang.Uncalculated})";
        }

        public CommissionReportDataViewModel GenerateCommissionReportData(CommissionReportParam param)
        {
            var rawData = _commissionReportRepository.GetAffiliateCommission(param);

            var byMonth = rawData.GroupBy(n => n.MonthDate)
                        .OrderByDescending(g => g.Key)
                        .Select(g => new AffiliateCommisionByMonth
                        {
                            Month = g.Key.Month,
                            DiplayMonth = g.Key.ToString("MMM-yyyy") + GetCalculatedNote(g.FirstOrDefault()?.CalculatedType),
                            Data = g.FirstOrDefault(i => i.ProductId == 0),
                            ProductData = g.Where(i => i.ProductId > 0).ToList()
                        }).ToList();

            var total = new AffiliateCommission();
            foreach (var item in byMonth.Select(item => item.Data))
            {
                item.CommissionRate *= PercentageUnit;
                item.SubCommRate *= PercentageUnit;

                total.TurnOver += item.TurnOver;
                total.GGR += item.GGR;
                total.PlatformFee += item.PlatformFee;
                total.DepositFee += item.DepositFee;
                total.WithdrawalFee += item.WithdrawalFee;
                total.Bonus += item.Bonus;
                total.Rebate += item.Rebate;
                total.Commission += item.Commission;
                total.P2pCommission += item.P2pCommission;
                total.SubAffCommission += item.SubAffCommission;
            }

            return new CommissionReportDataViewModel
            {
                Data = byMonth,
                Total = total
            };
        }
    }
}