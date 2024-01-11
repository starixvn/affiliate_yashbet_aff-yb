using Affiliate.YB.Models;
using Affiliate.YB.SpParams.Commands;
using Affiliate.YB.SpParams.Criteria;
using Fanex.Data.Repository;
using System;
using System.Threading.Tasks;

namespace Affiliate.YB.Repository
{
    public class TrackerRespository : ITrackerRepository
    {
        private readonly IDynamicRepository _repo;

        public TrackerRespository(IDynamicRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<TrackerInfo> GetLastedTrackerAsync(GetLastedImportTimeCriteria timeInfo)
            => await _repo.GetAsync<TrackerInfo>(timeInfo);

        public async Task ImportTimeTrackerAsync(InsertImportTrackerCommand timeInfo)
            => await _repo.ExecuteAsync(timeInfo);
    }
}
