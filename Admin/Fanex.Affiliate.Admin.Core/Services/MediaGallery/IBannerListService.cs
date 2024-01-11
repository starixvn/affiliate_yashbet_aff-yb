using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using System.Collections.Generic;
using System.Web;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public interface IBannerListService
    {
        EntityResult<BannerInfo> GetBannerList(BannerParam param);

        BannerInfo GetBannerDetail(int selectedBannerId, byte userType);

        ResultInfo InsertBanner(HttpPostedFileBase[] files, InsertUpdateMultipleBannersParam param);

        ResultInfo UpdateBanner(HttpPostedFileBase file, InsertUpdateMultipleBannersParam param);

        ResultInfo DeleteBanners(DeleteBannerParam param);

        IEnumerable<MediaGalleryFilterItem> GetSizeList(SizeParam param);
    }
}
