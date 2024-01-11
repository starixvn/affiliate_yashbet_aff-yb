using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;

namespace Fanex.Affiliate.Admin.Core.Repository.Affiliate
{
    public interface IAffiliateInfoRepository
    {
        EntityResult<AffiliateInfo> GetAffiliateList(AffiliateInfoParam param);

        ResultInfo ToggleFavorite(FavoriteParam param);

        ResultInfo ChangePassword(PasswordParam param);
    }
}