using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class NewsServiceTest
    {
        private readonly INewsRepository NewsRepo;
        private readonly IMemoryCacheService CacheService;

        public NewsServiceTest()
        {
            NewsRepo = Substitute.For<INewsRepository>();
            CacheService = Substitute.For<IMemoryCacheService>();
        }

        [Fact]
        public void GetLastestHotNews_WhenHaveInfoInCache_RepoNotCall()
        {
            var service = CreateService();
            CacheService.Get<LatestHotNews>(CacheKey.LatestHotNews).Returns(CreateLastetHotNews());

            service.GetLatestHotNews(false);

            NewsRepo.DidNotReceiveWithAnyArgs().GetLastestHotNews(false);
        }

        [Fact]
        public void GetLastestHotNews_WhenCacheNotHaveInfo_RepoCall()
        {
            var service = CreateService();
            CacheService.Get<LatestHotNews>(CacheKey.LatestHotNews).ReturnsNullForAnyArgs();
            NewsRepo.GetLastestHotNews(false).ReturnsForAnyArgs(CreateLastetHotNews());

            service.GetLatestHotNews(false);

            NewsRepo.ReceivedWithAnyArgs().GetLastestHotNews(false);
            CacheService.Received().Set(CacheKey.LatestHotNews, Arg.Compat.Any<LatestHotNews>());
        }

        [Fact]
        public void GetLastestHotNews_WhenBothRepoAndCacheNotHaveInfo_ReturnEmptyHotNewsList()
        {
            var service = CreateService();
            CacheService.Get<LatestHotNews>(CacheKey.LatestHotNews).ReturnsNullForAnyArgs();
            NewsRepo.GetLastestHotNews(false).ReturnsNullForAnyArgs();

            var result = service.GetLatestHotNews(false);

            CacheService.Received().Set(CacheKey.LatestHotNews, Arg.Compat.Any<LatestHotNews>());
            Assert.Empty(result.HotNewsList);
        }

        [Fact]
        public void GetMessageList_WhenCall_CallRepoWithCorrectParam()
        {
            var param = new MessageHistoryParam();
            var service = CreateService();

            service.GetMessageList(param);

            NewsRepo.Received().GetMessageList(param);
        }

        private LatestHotNews CreateLastetHotNews()
            => new LatestHotNews
            {
                HotNewsList = new Dictionary<string, string> { { "aa", "bb" } },
                IsImportant = true
            };

        private NewsService CreateService()
            => new NewsService(NewsRepo, CacheService);
    }
}
