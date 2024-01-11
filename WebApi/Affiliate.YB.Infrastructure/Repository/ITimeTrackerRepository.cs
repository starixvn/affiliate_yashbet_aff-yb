using Affiliate.YB.Models;
using Affiliate.YB.SpParams.Commands;
using Affiliate.YB.SpParams.Criteria;
using System.Threading.Tasks;

namespace Affiliate.YB.Repository
{
    public interface ITrackerRepository
    {
        Task<TrackerInfo> GetLastedTrackerAsync(GetLastedImportTimeCriteria timeInfo);

        Task ImportTimeTrackerAsync(InsertImportTrackerCommand timeInfo);

    }
}
