using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.Constants;
using Affiliate.YB.Jobs.RecurringTasks;
using Affiliate.YB.Models;
using Affiliate.YB.Repository;
using Affiliate.YB.Services;
using Affiliate.YB.SpParams.Commands;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace Affiliate.YB.Jobs.Tests
{
    public class FetchBetRecordTaskTests
    {
        private readonly ILicenseeApi Api;
        private readonly ITrackerService TimeTrackerService;
        private readonly ILogger Logger;
        private readonly IOptionsSnapshot<FetchServiceSetting> Configuration;
        private readonly IThirdPartyImportRepository Repo;

        private FetchServiceSetting Settings => Configuration.Get(ServiceConfigName.PULL_BET_RECORD_SETTING);

        public FetchBetRecordTaskTests()
        {
            TimeTrackerService = Substitute.For<ITrackerService>();
            Logger = Substitute.For<ILogger>();
            Configuration = Substitute.For<IOptionsSnapshot<FetchServiceSetting>>();
            Repo = Substitute.For<IThirdPartyImportRepository>();
            Api = Substitute.For<ILicenseeApi>();
        }

        private FetchBetRecordDataTask CreateFetchTask()
        {
            return new FetchBetRecordDataTask(Api, Repo, Logger, TimeTrackerService, Configuration);
        }

        private void SetUpStubConfiguration(int? maxTimeRange = null)
        {
            var config = FetchBetRecordTaskTestsHelper.GetConfiguration();
            config.MaxTimeRange = maxTimeRange ?? config.MaxTimeRange;
            Configuration.Get(ServiceConfigName.PULL_BET_RECORD_SETTING).Returns(config);
        }

        [Theory]
        [MemberData(nameof(FetchBetRecordTaskTestsHelper.TimeTrackerExceptionCannotHandle), MemberType = typeof(FetchBetRecordTaskTestsHelper))]
        public async Task FetchDataAsync_WhenCall_LoggerLogExceptionWhenTimeTrackerServiceThrowException(Exception ex)
        {
            SetUpStubConfiguration();
            var fetchTask = CreateFetchTask();
            TimeTrackerService.When(x => x.GetTrackerAsync(Arg.Any<string>(), t => t.CalledTime)).Do(x => throw ex);
            await fetchTask.FetchDataAsync();
            Logger.Received().Error(Arg.Is<string>(m => m.Contains(ex.Message)));
        }

        [Theory]
        [MemberData(nameof(FetchBetRecordTaskTestsHelper.LicenseeCalls), MemberType = typeof(FetchBetRecordTaskTestsHelper))]
        public async Task FetchDataAsync_WhenCall_NumberOfApiCallBaseOnMaxTimeRange(DateTime fromDate, DateTime toDate, int maxTimeRange, int numberOfTimeApiCall)
        {
            SetUpStubConfiguration(maxTimeRange);
            var reponse = FetchBetRecordTaskTestsHelper.GenerateApiResponse();
            Api.GetBetRecordAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<int>()).Returns(reponse);
            var fetchTask = CreateFetchTask();
            await fetchTask.FetchDataAsync(fromDate, toDate);
            await Api.ReceivedWithAnyArgs(numberOfTimeApiCall).GetBetRecordAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<int>());
        }

        [Theory]
        [MemberData(nameof(FetchBetRecordTaskTestsHelper.NoBetRecordItem), MemberType = typeof(FetchBetRecordTaskTestsHelper))]
        public async Task FetchDataAsync_WhenCallWithApiReturnNoItem_TimeTrackerNotSave(IEnumerable<TransBetRecordInfo> data)
        {
            SetUpStubConfiguration();
            Api.GetBetRecordAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<int>()).Returns(data);
            var fetchTask = CreateFetchTask();
            var fromDate = new DateTime(2020, 2, 2, 0, 0, 0);
            var toDate = new DateTime(2020, 2, 2, 0, 30, 0);
            await fetchTask.FetchDataAsync(fromDate, toDate);
            await TimeTrackerService.DidNotReceiveWithAnyArgs().SaveTrackerAsync(Arg.Any<string>(), Arg.Any<TrackerInfo>());
        }

        [Fact]
        public async Task FetchDataAsync_WhenCallWithApiReturnDataNeedPaging_InsertPagingDataMultipleTime()
        {
            var data = FetchBetRecordTaskTestsHelper.GenerateApiResponse(4000);
            var fromDate = new DateTime(2020, 2, 2, 0, 0, 0);
            var toDate = new DateTime(2020, 2, 2, 0, 30, 0);

            SetUpStubConfiguration();
            var numberOfRepo = Settings.SupportedPrefix.Count();
            Api.GetBetRecordAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<int>()).Returns(data);
            var fetchTask = CreateFetchTask();

            await fetchTask.FetchDataAsync(fromDate, toDate);

            await Repo.Received(2 * numberOfRepo).ImportRecordAsync(Arg.Is<InsertThirdPartyRecodInfoCommand>(r => CheckXmlElementIsLowerString(r.XmlData)));
        }

        private bool CheckXmlElementIsLowerString(string xmlData)
        {
            var props = typeof(TransBetRecordInfo).GetProperties();
            foreach (var pro in props)
            {
                if (!xmlData.Contains(pro.Name.ToLowerInvariant()))
                    return false;
            }
            return true;
        }
    }

    public static class FetchBetRecordTaskTestsHelper
    {
        public static IEnumerable<object[]> TimeTrackerExceptionCannotHandle => new List<object[]>
        {
            new object[]{ new Exception() },
            new object[]{ new InvalidCastException() }
        };

        public static IEnumerable<object[]> TimeTrackerHandlableException => new List<object[]>
        {
            new object[]{ new XmlException() },
            new object[]{ new FileNotFoundException() },
        };

        public static IEnumerable<object[]> LicenseeCalls => new List<object[]>
        {
            new object[] { new DateTime(2020, 2, 2, 0, 0, 0), new DateTime(2020, 2, 2, 0, 10, 0), 30, 0 },
            new object[] { new DateTime (2020, 2, 2, 0, 0, 0), new DateTime(2020, 2, 2, 1, 0, 0), 30, 2 },
            new object[] { new DateTime (2020, 2, 2, 0, 0, 0), new DateTime(2020, 2, 2, 1, 20, 0), 30, 2 }
        };

        public static IEnumerable<object[]> NoBetRecordItem => new List<object[]>
        {
            new object[] { null },
            new object[] { new List<TransBetRecordInfo>() }
        };

        public static FetchServiceSetting GetConfiguration() => new FetchServiceSetting
        {
            AdjustmentValue = 0,
            MaxTimeRange = 30,
            PullInfoCron = "*/30 * * * *",
            DataSource = "Table",
            SupportedPrefix = new[] { "test1", "test2" }
        };

        public static IEnumerable<TransBetRecordInfo> GenerateApiResponse(int count = 1)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                yield return new TransBetRecordInfo
                {
                    TpId = random.Next(100),
                    GameType = random.Next(10)
                };
            }
        }
    }
}