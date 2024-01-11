using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public interface ITextListService
    {
        EntityResult<TextInfo> GetTextList(TextParam param);

        TextInfo GetTextDetail(int selectedTextId, byte userType);

        ResultInfo InsertText(InsertUpdateTextParam param);

        ResultInfo UpdateText(InsertUpdateTextParam param);

        ResultInfo DeleteTexts(DeleteTextParam param);
    }
}
