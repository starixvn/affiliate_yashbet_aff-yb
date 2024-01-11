using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;

namespace Fanex.Affiliate.Admin.Core.Repository.Report
{
    public interface IPlayerGgrReportRepository
    {
        EntityResult<PlayerGgrReport> GetPlayerGgrReportData(PlayerGgrReportParam param);
    }
}
