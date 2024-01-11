using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;

namespace Fanex.Affiliate.Admin.Core.Services
{
    public interface IAffiliateService
    {
        AffiliateBasicInfo GetAffiliateBasicInfo(int? affId, string affName = null);

        (ResultInfo, string) ApproveAffiliate(ApproveAffiliateParam param);
    }
}
