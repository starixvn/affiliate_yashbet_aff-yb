using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _hotNewsRepository;
        private readonly IMemoryCacheService _cacheService;

        public NewsService(INewsRepository hotNewsRepository, IMemoryCacheService cacheService)
        {
            _hotNewsRepository = hotNewsRepository;
            _cacheService = cacheService;
        }

        public LatestHotNews GetLatestHotNews(bool isInternal)
        {
            var hotNews = _cacheService.Get<LatestHotNews>(CacheKey.LatestHotNews);

            if (hotNews == null)
            {
                hotNews = _hotNewsRepository.GetLastestHotNews(isInternal);
                _cacheService.Set(CacheKey.LatestHotNews, hotNews);
            }

            return hotNews ?? new LatestHotNews { HotNewsList = new Dictionary<string, string>() };
        }

        public IEnumerable<MessageHistory> GetMessageList(MessageHistoryParam param)
        {
            return _hotNewsRepository.GetMessageList(param);
        }
    }
}
