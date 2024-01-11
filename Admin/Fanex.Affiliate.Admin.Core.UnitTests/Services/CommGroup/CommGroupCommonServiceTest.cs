using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.CommGroup;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.CommGroup
{
    public class CommGroupCommonServiceTest
    {
        [Fact]
        public void CreateCommissionGroup_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var param = CreateValidCreateUpdateCommissionGroupParam();
            var commGroupRepository = new Mock<ICommissionGroupRepository>();
            commGroupRepository.Setup(comm => comm.CreateCommissionGroup(param)).Returns(new ResultInfo());

            var service = CreateCommGroupCommonService(commGroupRepository.Object);

            // Action
            var result = service.CreateCommissionGroup(param);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var param = CreateValidCreateUpdateCommissionGroupParam();
            var commGroupRepository = new Mock<ICommissionGroupRepository>();
            commGroupRepository.Setup(comm => comm.UpdateCommissionGroup(param)).Returns(new ResultInfo());

            var service = CreateCommGroupCommonService(commGroupRepository.Object);

            // Action
            var result = service.UpdateCommissionGroup(param);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void GetDeductionList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var commGroupRepository = new Mock<ICommissionGroupRepository>();
            var expectedResult = new List<DeductionItem>
                    {
                        new DeductionItem
                        {
                            DeductedPercentage= 0, DeductionId= 14, DeductionName = "ExchangeCommissionShare"
                        }
                    };
            commGroupRepository.Setup(comm => comm.GetDeductionList()).Returns(expectedResult);
            var service = CreateCommGroupCommonService(commGroupRepository.Object);

            // Action
            var result = service.GetDeductionList();

            // Assert
            Assert.Single(result);
        }

        private static CommGroupCommonService CreateCommGroupCommonService(
            ICommissionGroupRepository commGroupRepository = null)
        {
            var fakeCommGroupRepository = commGroupRepository ?? Mock.Of<ICommissionGroupRepository>();

            return new CommGroupCommonService(fakeCommGroupRepository);
        }

        private static CreateUpdateCommissionGroupParam CreateValidCreateUpdateCommissionGroupParam()
        {
            return new CreateUpdateCommissionGroupParam
            {
                DepositFee = new TierDeductionSettingParam
                {
                    DeductedPercentage = 0.02m,
                    DeductionId = 13,
                    ProductIds = "0",
                    IsDeductedPercent = true,
                    DeductedAmount = ""
                },
                WithdrawalFee = new TierDeductionSettingParam
                {
                    DeductedPercentage = 0.02m,
                    DeductionId = 14,
                    ProductIds = "0",
                    IsDeductedPercent = true,
                    DeductedAmount = ""
                },
                PlatformFeeList = new List<TierDeductionSettingParam>
                {
                    new TierDeductionSettingParam
                    {
                        DeductedPercentage = 0.17m,
                        DeductionId = 10,
                        ProductIds = "5,6,7,8,9,10,11,12,13,14,15,16,17,18",
                        IsDeductedPercent = true,
                        DeductedAmount = ""
                    }
                },
                P2pDeductionList = new List<P2PDeductionSettingParam>
                {
                    new P2PDeductionSettingParam
                    {
                        DeductedPercentage = 0.17m,
                        DeductionId = 10,
                        ProductIds = "1,3,4",
                        Commission = 0.1m
                    }
                },
                TierSettingList = new List<TierSettingParam>
                {
                    new TierSettingParam
                    {
                        Commission = 0.01m,
                        MinAmount = 0,
                        MinAp = 0,
                        TierId = 1
                    },
                    new TierSettingParam
                    {
                        Commission = 0.10m,
                        MinAmount = 101,
                        MinAp = 101,
                        TierId = 2
                    }
                }
            };
        }
    }
}
