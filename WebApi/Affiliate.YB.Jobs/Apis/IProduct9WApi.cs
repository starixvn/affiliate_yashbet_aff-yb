using Affiliate.YB.Models;
using Affiliate.YB.Params;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.Apis
{
    public interface IProduct9WApi
    {
        [Get("/Product9W/GetTpReport")]
        Task<IEnumerable<TpReport9W>> GetTpReportAsync([Query] BaseParam param);

        [Get("/Product9W/GetTpWager")]
        Task<IEnumerable<TpWager9W>> GetTpWagerAsync([Query] BaseParam param);
    }
}
