using Affiliate.YB.Jobs.ViewModels;
using Affiliate.YB.Models;
using Affiliate.YB.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepullController : ControllerBase
    {
        public ITrackerService TrackerService { get; }

        public RepullController(ITrackerService trackerService)
        {
            TrackerService = trackerService ?? throw new ArgumentNullException(nameof(trackerService));
        }

        [HttpPost("UpdateTrackerToRepull")]
        public async Task<ResultInfo> UpdateTrackerToRepull(TrackerModel tracker)
        {
            var result = new ResultInfo { IsSuccess = false };
            var dataSource = tracker?.DataSource;
            if (!string.IsNullOrWhiteSpace(dataSource))
            {
                var info = new TrackerInfo
                {
                    CalledTime = tracker.Date,
                    IsReset = true,
                    KeyValue = tracker.Value
                };

                try
                {
                    await TrackerService.SaveTrackerAsync(dataSource, info);
                    result.IsSuccess = true;
                    result.ErrMsg = String.Empty;
                }
                catch (Exception ex)
                {
                    result.ErrMsg = ex.Message;
                }
            }

            return result;
        }
    }
}
