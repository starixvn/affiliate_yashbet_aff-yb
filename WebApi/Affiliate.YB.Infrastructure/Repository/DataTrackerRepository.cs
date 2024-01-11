

using Affiliate.YB.DTOs;
using Affiliate.YB.SpParams;
using Affiliate.YB.SpParams.Commands;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Repository
{
    public class DataTrackerRepository : IDataTrackerRepository
    {
        private readonly IDynamicRepository _repo;

        public DataTrackerRepository(IDynamicRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IEnumerable<DataTrackerItem>> FetchDataTracker(DataTrackerCriteria criteria)
           => _repo.FetchAsync<DataTrackerItem>(criteria);
        

        public Task UpdateDataTrackerAsync(UpdateDataTrackerCommand command)
            => _repo.ExecuteAsync(command);
    }
}
