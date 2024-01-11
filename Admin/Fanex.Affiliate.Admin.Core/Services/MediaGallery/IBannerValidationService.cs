using Fanex.Affiliate.Admin.Core.DTO;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public interface IBannerValidationService
    {
        ResultInfo ValidateFile(string fileName, int contentLength);

        ResultInfo CheckFileInFolder(string folder, string filePath);
    }
}
