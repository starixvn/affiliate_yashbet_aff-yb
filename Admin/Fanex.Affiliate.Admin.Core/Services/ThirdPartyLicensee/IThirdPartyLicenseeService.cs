using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using System.Threading.Tasks;

namespace Fanex.Affiliate.Admin.Core.Services.ThirdPartyLicensee
{
    public interface IThirdPartyLicenseeService
    {
        Task SendAffiliateInfo(AffiliateBasicInfo affInfo);
    }
}
