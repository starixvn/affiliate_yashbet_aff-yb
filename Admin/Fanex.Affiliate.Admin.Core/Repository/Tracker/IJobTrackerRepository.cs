
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Tracker;

namespace Fanex.Affiliate.Admin.Core.Repository.Tracker
{
    public interface IJobTrackerRepository
    {
        (int Id, ResultInfo Result) InsertDataTracker(InsertDataTrackerDTO param);

        ResultInfo UpdateDataTracker(UpdateDataTrackerDTO param);
    }
}
