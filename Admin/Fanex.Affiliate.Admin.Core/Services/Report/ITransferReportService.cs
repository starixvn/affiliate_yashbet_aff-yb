using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;

namespace Fanex.Affiliate.Admin.Core.Services.Report
{
    public interface ITransferReportService
    {
        EntityResult<TransferReport> GetTransferReportData(TransferReportParam param);
    }
}
