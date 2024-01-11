using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ICommissionReportService
    {
        CommissionReportDataViewModel GenerateCommissionReportData(CommissionReportParam param);
    }
}
