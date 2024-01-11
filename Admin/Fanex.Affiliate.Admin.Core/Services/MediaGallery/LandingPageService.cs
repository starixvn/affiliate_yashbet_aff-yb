using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Repository.MediaGallery;
using Fanex.Affiliate.Admin.Core.Utilities;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public class LandingPageService : ILandingPageService
    {
        private const string RestrictedCharacterPattern = "[;/\\?:@=&\"<>#%\\{\\}\\|\\\\\\^~\\[\\]`\\s+]";
        private const string RestricedCharacterErrorMessage = "Special characters are not allowed:\n the blank/empty space and ; / ? : @ = & \" < > # % { } | \\ ^ ~ [ ] `";

        private readonly ILandingPageRepository _landingPageRepository;

        public LandingPageService(ILandingPageRepository landingPageRepository)
        {
            _landingPageRepository = landingPageRepository;
        }

        public IEnumerable<LandingPage> GetLandingPageList()
        {
            return _landingPageRepository.GetLandingPageList();
        }

        public LandingPage GetLandingPageById(int pageId)
        {
            return _landingPageRepository.GetLandingPageById(pageId);
        }

        public ResultInfo InsertLandingPage(InsertUpdateLandingPageParam param)
        {
            if (HasRestrictedCharacter(param.PageCode))
            {
                return new ResultInfo
                {
                    Code = ErrCode.GeneralErrorCode,
                    Message = RestricedCharacterErrorMessage
                };
            }

            return _landingPageRepository.InsertLandingPage(param);
        }

        public ResultInfo UpdateLandingPage(InsertUpdateLandingPageParam param)
        {
            if (HasRestrictedCharacter(param.PageCode))
            {
                return new ResultInfo
                {
                    Code = ErrCode.GeneralErrorCode,
                    Message = RestricedCharacterErrorMessage
                };
            }

            return _landingPageRepository.UpdateLandingPage(param);
        }

        public ResultInfo DeleteLandingPage(DeleteLandingPageParam param)
        {
            return _landingPageRepository.DeleteLandingPage(param);
        }

        private static bool HasRestrictedCharacter(string input)
        {
            return RegexHelper.IsMatch(input, RestrictedCharacterPattern);
        }
    }
}