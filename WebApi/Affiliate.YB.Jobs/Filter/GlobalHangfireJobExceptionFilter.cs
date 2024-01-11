using Fanex.Athena.CronJobs.App.Helpers;
using Fanex.Logging;
using Hangfire.Common;
using Hangfire.States;
using System;
using System.Text;

namespace Affiliate.YB.Jobs.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface)]
    public class GlobalHangfireJobExceptionFilterAttribute : JobFilterAttribute, IElectStateFilter
    {
        public GlobalHangfireJobExceptionFilterAttribute()
        {
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                var exception = failedState.Exception;
                Logger.Log.Error(GenerateJobException(context), exception);
            }
        }

        private static string GenerateJobException(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Job Name: {context.BackgroundJob.Job.Type.Name}");
            stringBuilder.AppendLine($"Job Id: {context.BackgroundJob.Id}");
            stringBuilder.AppendLine($"Created Time: {context.BackgroundJob.CreatedAt}");
            stringBuilder.AppendLine(ErrorMessageHelper.GenerateMessage(failedState.Exception));
            return stringBuilder.ToString();
        }
    }
}
