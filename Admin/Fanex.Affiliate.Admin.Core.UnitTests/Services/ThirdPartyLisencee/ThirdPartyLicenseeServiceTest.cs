using Fanex.Affiliate.Admin.Core.Apis;
using Fanex.Affiliate.Admin.Core.Configurations;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.DTO.Api;
using Fanex.Affiliate.Admin.Core.DTO.Tracker;
using Fanex.Affiliate.Admin.Core.Repository.Tracker;
using Fanex.Affiliate.Admin.Core.Services.ThirdPartyLicensee;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.ThirdPartyLisencee
{
    public class ThirdPartyLicenseeServiceTest
    {
        private readonly IJobTrackerRepository _trackerRepository;
        private readonly ILicenseeApi _licenseeApi;
        private readonly IAppSetting _appSetting;

        public ThirdPartyLicenseeServiceTest()
        {
            _trackerRepository = Substitute.For<IJobTrackerRepository>();
            _licenseeApi = Substitute.For<ILicenseeApi>();
            _appSetting = Substitute.For<IAppSetting>();
        }

        [Fact]
        public async Task SendAffiliateInfo_WhenCallWithThrowException_ShouldCallUpdateDataTracker()
        {
            //Arrange
            MockingHelper.SetUpHttpContextSession();
            MockingHelper.SetUpLogEngineEnviroment();

            _trackerRepository.InsertDataTracker(Arg.Any<InsertDataTrackerDTO>()).Returns((123, ResultInfo.Success));
            _licenseeApi.InsertAffiliateInfo(Arg.Compat.Any<ApiAffiliateInfo>()).Returns(Task.FromResult(new ApiResultInfo()));

            //Action
            var service = CreateThirdPartyLicenseeService();
            await service.SendAffiliateInfo(new AffiliateBasicInfo());

            //Assert
            _trackerRepository.Received().UpdateDataTracker(Arg.Compat.Any<UpdateDataTrackerDTO>());
        }

        [Fact]
        public async Task SendAffiliateInfo_WhenCallWithInsertDataTrackerFailed_ShouldNotCallUpdateDataTracker()
        {
            //Arrange
            _trackerRepository.InsertDataTracker(Arg.Any<InsertDataTrackerDTO>()).Returns((123, ResultInfo.GenerateErr("Error")));

            //Action
            var service = CreateThirdPartyLicenseeService();
            await service.SendAffiliateInfo(new AffiliateBasicInfo());

            //Assert
            _trackerRepository.DidNotReceive().UpdateDataTracker(Arg.Compat.Any<UpdateDataTrackerDTO>());
        }

        [Fact]
        public async Task SendAffiliateInfo_WhenCallWithSuccessful_ShouldCallUpdateDataTracker()
        {
            //Arrange
            MockingHelper.SetUpHttpContextSession();
            MockingHelper.SetUpLogEngineEnviroment();

            _trackerRepository.InsertDataTracker(Arg.Any<InsertDataTrackerDTO>()).Returns((123, ResultInfo.Success));
            _licenseeApi.InsertAffiliateInfo(Arg.Compat.Any<ApiAffiliateInfo>()).Returns(Task.FromResult(new ApiResultInfo { IsSuccess = true }));

            //Action
            var service = CreateThirdPartyLicenseeService();
            await service.SendAffiliateInfo(new AffiliateBasicInfo());

            //Assert
            _trackerRepository.Received().UpdateDataTracker(Arg.Compat.Any<UpdateDataTrackerDTO>());
        }

        private ThirdPartyLicenseeService CreateThirdPartyLicenseeService()
            => new ThirdPartyLicenseeService(_trackerRepository, _licenseeApi, _appSetting);
    }
}
