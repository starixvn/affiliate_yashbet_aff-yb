using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Resources;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Utilities.MediaGallery
{
    public static class BannerHelper
    {
        private static readonly string[] ValidExtensions = {
            ".gif", ".jpg", ".png", ".jpeg"
        };

        public static bool IsValidExtension(string extension)
        {
            return ValidExtensions.Contains(extension.ToLowerInvariant());
        }

        public static string GetSize(int width, int height)
        {
            return string.Format("{0}x{1}", width, height);
        }

        public static ResultInfo CreateErrorResult()
        {
            return new ResultInfo
            {
                Code = ErrCode.GeneralErrorCode,
                Message = Lang.InvalidBannerExtensionFile
            };
        }
    }
}
