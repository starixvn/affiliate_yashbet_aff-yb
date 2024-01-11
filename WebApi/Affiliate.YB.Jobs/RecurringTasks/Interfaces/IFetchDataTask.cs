using Hangfire;
using System;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.Interfaces
{
    public interface IFetchDataTask
    {
        [DisableConcurrentExecution(60)]
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 1)]
        Task FetchDataAsync();
    }

    public interface IFetchDataByTimeRange
    {
        Task FetchDataAsync(DateTime fromDate, DateTime toDate);
    }
}
