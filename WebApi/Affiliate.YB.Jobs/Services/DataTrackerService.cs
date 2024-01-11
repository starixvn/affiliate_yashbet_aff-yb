using Affiliate.YB.Core;
using Affiliate.YB.DTOs;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Affiliate.YB.SpParams;
using Affiliate.YB.SpParams.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.Services
{
    public class DataTrackerService : IDataTrackerService
    {
        private readonly IDataTrackerRepository _repository;

        public DataTrackerService(IDataTrackerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DataTrackerItem>> FetchTrackerAsync(IEnumerable<IPrefixProvider> prefixProviders, IEnumerable<int> dataTypes, IEnumerable<int> statuses)
        {
            var listTracker = new List<DataTrackerItem>();
            foreach (var prefixProvider in prefixProviders)
            {
                foreach (var dataType in dataTypes)
                {
                    foreach (var status in statuses)
                    {
                        listTracker.AddRange(await FetchAndAssignValue(new DataTrackerCriteria(prefixProvider)
                        {
                            PushDataType = dataType,
                            StatusId = status
                        }, prefixProvider));
                    }
                }             
            }
            return listTracker;
        }

        public Task UpdateTrackerStatusAsync(DataTrackerItem item, int status)
        {
            return _repository.UpdateDataTrackerAsync(new UpdateDataTrackerCommand(item.PrefixProvider)
            {
                StatusId = status,
                PushTrackerId = item.PushTrackerId
            });
        }

        private async Task<IEnumerable<DataTrackerItem>> FetchAndAssignValue(DataTrackerCriteria criteria, IPrefixProvider prefixProvider)
        {
            var items = await _repository.FetchDataTracker(criteria);
            foreach (var item in items)
            {
                item.PrefixProvider = prefixProvider;
            }
            return items;
        }
    }
}
