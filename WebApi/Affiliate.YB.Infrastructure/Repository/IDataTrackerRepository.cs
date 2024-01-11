

using System.Threading.Tasks;
using Affiliate.YB.SpParams.Commands;
using Affiliate.YB.SpParams;
using Affiliate.YB.DTOs;
using System.Collections.Generic;

namespace Affiliate.YB.Repository
{
    public interface IDataTrackerRepository
    {
        Task UpdateDataTrackerAsync(UpdateDataTrackerCommand command);

        Task<IEnumerable<DataTrackerItem>> FetchDataTracker(DataTrackerCriteria criteria);
    }
}
