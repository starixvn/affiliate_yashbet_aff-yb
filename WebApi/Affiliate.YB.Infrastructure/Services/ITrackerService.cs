using Affiliate.YB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Services
{
    public interface ITrackerService
    {
        Task<TrackerInfo> GetTrackerAsync<TKey>(string source, Func<TrackerInfo, TKey> minKeySelector);

        Task<TrackerInfo> GetTrackerAsync<TKey>(string source, Func<TrackerInfo, TKey> minKeySelector, IComparer<TKey> comparer);

        Task SaveTrackerAsync(string source, TrackerInfo tracker);
    }
}