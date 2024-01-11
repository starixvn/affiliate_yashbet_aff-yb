using Hangfire;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks.Interfaces
{
    public interface IRetryDataTask
    {
        [DisableConcurrentExecution(60)]
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 1)]
        Task HandleAsync();
    }
}
