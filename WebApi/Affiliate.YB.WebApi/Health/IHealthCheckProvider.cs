using Affiliate.YB.WebApi.Models.HelthCheck;
using System.Threading.Tasks;

namespace Affiliate.YB.WebApi.Health
{
    public interface IHealthCheckProvider
    {
        /// <summary>
        /// Returns the health heck info.
        /// </summary>
        Task<HealthCheckItemResult> GetHealthCheckAsync();

        /// <summary>
        /// Defines the order of this provider in the results.
        /// </summary>
        int SortOrder { get; }
    }
}
