using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public interface ILandingPageService
    {
        IEnumerable<LandingPage> GetLandingPageList();

        LandingPage GetLandingPageById(int pageId);

        ResultInfo InsertLandingPage(InsertUpdateLandingPageParam param);

        ResultInfo UpdateLandingPage(InsertUpdateLandingPageParam param);

        ResultInfo DeleteLandingPage(DeleteLandingPageParam param);
    }
}
