using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository.Report;
using Fanex.Affiliate.Admin.Core.Utilities;
using System;

namespace Fanex.Affiliate.Admin.Core.Services.Report
{
    public class AffiliateReportService : IAffiliateReportService
    {
        private readonly IAffiliateReportRepository _affiliateReportRepository;

        public AffiliateReportService(IAffiliateReportRepository affiliateReportRepository)
        {
            _affiliateReportRepository = affiliateReportRepository;
        }

        public EntityResult<AffiliateReport> GetMainReportData(AffiliateReportParam param)
        {
            if (!IsValidParam(param))
            {
                return new EntityResult<AffiliateReport>();
            }

            return _affiliateReportRepository.GetMainReportData(param);
        }

        public EntityResult<AffiliateReport> GetDetailReportData(AffiliateReportParam param)
        {
            return _affiliateReportRepository.GetDetailReportData(param);
        }

        public DateTime GetReportCutOffDate()
        {
            return _affiliateReportRepository.GetReportCutOffDate();
        }

        private static bool IsValidParam(AffiliateReportParam param)
        {
            const int ValidMonthArrange = 5;
            return !string.IsNullOrEmpty(param.ProductIds) && DateTimeHelper.IsValidDateRange(param.FromDate, param.ToDate, ValidMonthArrange);
        }
    }
}