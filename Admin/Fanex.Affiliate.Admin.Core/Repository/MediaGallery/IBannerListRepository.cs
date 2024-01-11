using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.MediaGallery
{
    public interface IBannerListRepository
    {
        EntityResult<BannerInfo> GetBannerList(BannerParam param);

        BannerInfo GetBannerDetail(int selectedBannerId, byte userType);

        ResultInfo InsertBanner(InsertUpdateMultipleBannersParam param);

        ResultInfo UpdateBanner(InsertUpdateMultipleBannersParam param);

        ResultInfo DeleteBanners(DeleteBannerParam param);

        IEnumerable<MediaGalleryFilterItem> GetSizeList(SizeParam param);
    }
}
