using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Utilities;
using Fanex.Affiliate.Admin.Core.Utilities.MediaGallery;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public class BannerValidationService : IBannerValidationService
    {
        private readonly IAppSetting _appSetting;
        private readonly ISystemFile _systemFile;
        public BannerValidationService(
            IAppSetting appSetting,
            ISystemFile systemFile)
        {
            _appSetting = appSetting;
            _systemFile = systemFile;
        }

        public ResultInfo ValidateFile(string fileName, int contentLength)
        {
            if (!BannerHelper.IsValidExtension(_systemFile.GetExtension(fileName)))
            {
                return BannerHelper.CreateErrorResult();
            }

            if (contentLength > _appSetting.ImageSize)
            {
                return new ResultInfo
                {
                    Code = ErrCode.GeneralErrorCode,
                    Message = Lang.InvalidBannerSize
                };
            }

            return new ResultInfo();
        }

        public ResultInfo CheckFileInFolder(string folder, string filePath)
        {
            var result = new ResultInfo();
            foreach (var path in _appSetting.BannerFolders)
            {
                result = _systemFile.CreateFolder(path, folder);
                if (!result.IsSuccessful)
                {
                    return result;
                }

                result = _systemFile.CheckExistedFile(path, filePath);
                if (!result.IsSuccessful)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
