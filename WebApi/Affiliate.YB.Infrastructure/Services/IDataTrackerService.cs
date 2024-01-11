

using Affiliate.YB.Core;
using Affiliate.YB.DTOs;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Services
{
    public interface IDataTrackerService
    {
        Task<IEnumerable<DataTrackerItem>> FetchTrackerAsync(IEnumerable<IPrefixProvider> prefixProviders , IEnumerable<int> dataTypes, IEnumerable<int> statuses);

        Task UpdateTrackerStatusAsync(DataTrackerItem item, int status);
    }
}
