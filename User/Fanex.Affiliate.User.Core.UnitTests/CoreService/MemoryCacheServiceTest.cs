using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Caching;
using NSubstitute;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.CoreService
{
    public class MemoryCacheServiceTest
    {
        private readonly ICache Cache;

        public MemoryCacheServiceTest()
        {
            Cache = Substitute.For<ICache>();
        }

        [Fact]
        public void Contains_WhenCall_CallCache()
        {
            var service = CreateService();

            service.Contains("abc");

            Cache.Received().Contain("abc");
        }

        [Fact]
        public void Get_WhenCall_ReturnDeepClone()
        {
            var service = CreateService();
            Cache.TryGet("abc", out TestModel _).Returns(true);

            var result = service.Get<TestModel>("abc");

            Assert.Null(result);
        }

        [Fact]
        public void Get_WhenCall_ReturnValue()
        {
            var service = CreateService();
            Cache.TryGet("abc", out TestModel _).Returns(prs =>
            {
                prs[1] = new TestModel();
                return false;
            });

            var result = service.Get<TestModel>("abc");

            Assert.NotNull(result);
        }

        [Fact]
        public void Set_WhenCall_CacheCallWithDeepCloneValue()
        {
            var service = CreateService();
            TestModel cacheValue = null;
            var model = new TestModel
            {
                PropNumber = 1,
                PropString = "abc"
            };
            Cache.When(c => c.Set(Arg.Compat.Any<string>(), Arg.Compat.Any<TestModel>(), Arg.Compat.Any<CacheItemOptions>()))
                .Do(c =>
                {
                    cacheValue = c.Args()[1] as TestModel;
                });

            service.Set("abc", model, 120);

            Assert.NotNull(cacheValue);
            Assert.NotSame(cacheValue, model);
        }

        [Fact]
        public void Delete_WhenCall_CacheCallRemove()
        {
            var service = CreateService();

            service.Delete("abc");

            Cache.Received().Remove("abc");
        }

        private MemoryCacheService CreateService()
            => new MemoryCacheService(Cache);

        private class TestModel
        {
            public int PropNumber { get; set; }

            public string PropString { get; set; }
        }
    }
}
