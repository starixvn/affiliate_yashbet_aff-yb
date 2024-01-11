using Affiliate.YB.DTO;
using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.RecurringTasks;
using Affiliate.YB.Models;
using Affiliate.YB.Repository;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Affiliate.YB.Jobs.UnitTests
{
    public class FetchAffCustomerDataTaskTests
    {
        private readonly IMemberImportRepository Repo;
        private readonly ILicenseeApi Api;
        private readonly ILogger Logger;
        private readonly IOptions<LicenseeSiteSetting> Config;

        public FetchAffCustomerDataTaskTests()
        {
            Repo = Substitute.For<IMemberImportRepository>();
            Api = Substitute.For<ILicenseeApi>();
            Logger = Substitute.For<ILogger>();
            Config = Substitute.For<IOptions<LicenseeSiteSetting>>();
        }

        private FetchAffCustomerDataTask CreateFetchTask()
        {
            Config.Value.Returns(FetchCustomerDataTaskTestsHelper.Setting);
            return new FetchAffCustomerDataTask(Api, Repo, Logger, Config);
        }

        private void SetUpRepoReturnSiteName(LicenseeSiteSetting setting)
        {
            foreach (var site in setting.SupportedSites)
            {
                var prefix = setting.GetCodePrefix(site);
                Repo.GetSiteNameAsync(Arg.Is<SiteNameInfo>(s => s.GetSettingKey().StartsWith(prefix))).Returns(site);
            }
        }

        [Theory]
        [MemberData(nameof(FetchCustomerDataTaskTestsHelper.NotConfigSupportedSiteConfiguration), MemberType = typeof(FetchCustomerDataTaskTestsHelper))]
        public async Task FetchDataAsync_WhenCallWithNoHaveSupportSite_NotThrowAnyThing(LicenseeSiteSetting setting)
        {
            var fetchTask = CreateFetchTask();
            Config.Value.Returns(setting);
            await fetchTask.FetchDataAsync();
            Assert.True(true);
        }

        [Fact]
        public async Task TaskFetchDataAsync_WhenCall_RepositoryQueryLastedTimeRecieveCorrectPrefixSetting()
        {
            var setting = FetchCustomerDataTaskTestsHelper.Setting;
            var fetchTask = CreateFetchTask();
            Config.Value.Returns(setting);
            SetUpRepoReturnSiteName(setting);

            await fetchTask.FetchDataAsync();

            foreach (var site in setting.SupportedSites)
            {
                LicenseeSiteMapping map = setting.LicenseeSiteMapping.FirstOrDefault(m => m.SiteName.Equals(site, StringComparison.InvariantCultureIgnoreCase));
                var prefix = map == null ? site : map.SettingCodePrefix;

                await Repo.Received().GetLastedMemberImportedTimeAsync(Arg.Is<MemberImportedTimeInfo>(i => i.GetSettingKey().Contains(prefix)));
            }

        }

        [Fact]
        public async Task FetchDataAsync_WhenCallWithNoLastedDateQueryFromRepo_LoggerLogInvalidOperationException()
        {
            var fetchTask = CreateFetchTask();
            Repo.When(r => r.GetLastedMemberImportedTimeAsync(Arg.Any<MemberImportedTimeInfo>()))
                .Do(x => throw new Exception("test"));
            await fetchTask.FetchDataAsync();

            await Logger.Received().ErrorAsync(Arg.Is<string>(s => s.Contains("Can't find lasted importing member date"))
                , Arg.Is<Exception>(ex => ex.GetType() == typeof(InvalidOperationException)));
        }

        [Fact]
        public async Task FetchDataAsync_WhenCallWithAtLeastOneSuccessQueryLastedTime_ApiCall()
        {
            var date = new DateTime(2020, 2, 2);
            var fetchTask = CreateFetchTask();
            Repo.When(r => r.GetLastedMemberImportedTimeAsync(Arg.Is<MemberImportedTimeInfo>(m => m.GetSettingKey().Contains("YB") || m.GetSettingKey().Contains("Bet88"))))
                .Do(x => throw new Exception());
            Repo.GetLastedMemberImportedTimeAsync(Arg.Is<MemberImportedTimeInfo>(m => m.GetSettingKey().Contains("YB")))
                .Returns(date);

            await fetchTask.FetchDataAsync();

            await Repo.ReceivedWithAnyArgs(3).GetLastedMemberImportedTimeAsync(Arg.Any<MemberImportedTimeInfo>());
            await Logger.ReceivedWithAnyArgs(2).ErrorAsync(Arg.Any<string>(), Arg.Any<Exception>());
            await Api.Received(1).GetAffCustomerAsync(date);
        }

        [Fact]
        public async Task FetchDataAsync_WhenCall_ApiReceiveMinDate()
        {
            var minDate = new DateTime(2020, 2, 2);
            var anotherDate = new DateTime[] { minDate.AddDays(1), minDate.AddHours(2) };
            var fetchTask = CreateFetchTask();
            Repo.GetLastedMemberImportedTimeAsync(Arg.Any<MemberImportedTimeInfo>()).Returns(anotherDate[0], minDate, anotherDate[1]);

            await fetchTask.FetchDataAsync();

            await Api.Received().GetAffCustomerAsync(minDate);
        }

        [Fact]
        public async Task FetchDataAsync_WhenApiReturnNothing_LogNoData()
        {
            var date = new DateTime(2020, 2, 2);
            var fetchTask = CreateFetchTask();
            Api.GetAffCustomerAsync(Arg.Any<DateTime>()).Returns(new List<AffCustomerModel>());
            await fetchTask.FetchDataAsync(date);

            await Logger.Received().InfoAsync(Arg.Is<string>(s => s.Contains($"No Data on date {date}")));
        }

        [Fact]
        public async Task FetchDataAsync_WhenApiReturnCustomersThatNotBelongConfigurableSite_LogError()
        {
            var date = new DateTime(2020, 2, 2);
            var siteNotInConfig = FetchCustomerDataTaskTestsHelper.SiteNotInConfigSetting;
            var fetchTask = CreateFetchTask();
            var customers = FetchCustomerDataTaskTestsHelper.GetSiteCustomer(siteNotInConfig);
            Api.GetAffCustomerAsync(Arg.Any<DateTime>()).Returns(customers);

            await fetchTask.FetchDataAsync(date);

            await Logger.Received().InfoAsync(Arg.Is<string>(s => s.Contains($"No Import Setting for Prefix")));
        }
    }

    internal static class FetchCustomerDataTaskTestsHelper
    {
        public static IEnumerable<object[]> NotConfigSupportedSiteConfiguration = new List<object[]>
        {
            new object[] { new LicenseeSiteSetting { SupportedSites = null } },
            new object[] { new LicenseeSiteSetting { SupportedSites = new List<string> ()} }
        };

        public static LicenseeSiteSetting Setting => new LicenseeSiteSetting
        {
            SupportedSites = new List<string> { "YASHBET", "Bet88" },
            LicenseeSiteMapping = new List<LicenseeSiteMapping>
                {
                    new LicenseeSiteMapping { SettingCodePrefix = "YB", SiteName = "YASHBET" }
                }
        };

        public static IEnumerable<AffCustomerModel> Customers => new List<AffCustomerModel>
        {
            new AffCustomerModel{ SiteId = 1, SiteName = "YASHBET", CustId = new Random().Next() },
            new AffCustomerModel{ SiteId = 1, SiteName = "YASHBET", CustId = new Random().Next() },
            new AffCustomerModel{ SiteId = 2, SiteName = "Bet88", CustId = new Random().Next() },
            new AffCustomerModel{ SiteId = 2, SiteName = "Bet88", CustId = new Random().Next() },
            new AffCustomerModel{ SiteId = 2, SiteName = "Bet88", CustId = new Random().Next() },
            new AffCustomerModel{ SiteId = 3, SiteName = SiteNotInConfigSetting, CustId = new Random().Next() },
            new AffCustomerModel{ SiteId = 3, SiteName = SiteNotInConfigSetting, CustId = new Random().Next() },
        };

        public static string SiteNotInConfigSetting => "abc";

        public static IEnumerable<AffCustomerModel> GetSiteCustomer(string siteName) =>
            Customers.Where(c => c.SiteName.Equals(siteName, StringComparison.InvariantCultureIgnoreCase));
    }
}
