using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Services.CommGroup;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services
{
    public class CommissionGroupServiceTest
    {
        [Fact]
        public void GetCommissionGroupList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new GetCommissionGroupParam();
            var commissionGroupResultEntity = new CommissionGroupResultEntity
            {
                CommGroupList = new List<DTO.CommissionGroup.CommGroup>
                {
                    new DTO.CommissionGroup.CommGroup
                    {
                        RowIdx = 1,
                        GroupId = 101,
                        GroupName = "Default",
                        CreatedDate = new DateTime(2018, 7, 11),
                        Creator = "Admin",
                        Description = "Description",
                        NoAffs = 2
                    }
                },
                P2pDeductionSettingList = new List<DeductionSetting>
                {
                    new DeductionSetting
                    {
                        GroupId = 101,
                        DeductionId = 1,
                        DeductionName = "DeductionName",
                        ProductIds = "1,2,3",
                        DeductedPercentage = 0.7m,
                        Commission = 0.1m
                    }
                },
                TierDeductionSettingList = new List<DeductionSetting>
                {
                    new DeductionSetting
                    {
                        GroupId = 101,
                        DeductionId = 1,
                        DeductionName = "DeductionName",
                        ProductIds = "1,2,3",
                        DeductedPercentage = 0.7m
                    },
                    new DeductionSetting
                    {
                        GroupId = 101,
                        DeductionId = 13,
                        DeductionName = "ExchangeGgrShare",
                        ProductIds = "19",
                        DeductedPercentage = 0.0m
                    },
                    new DeductionSetting
                    {
                        GroupId = 101,
                        DeductionId = 11,
                        DeductionName = "BankingFee",
                        ProductIds = "0",
                        DeductedPercentage = 0.07m
                    }
                },
                TierSettingList = new List<TierSetting>
                {
                    new TierSetting
                    {
                        GroupId = 101,
                        TierId = 1,
                        MinAmount = 0m,
                        MinAp = 0,
                        Commission = 0.2m
                    }
                }
            };
            var commGroupRepository = new Mock<ICommissionGroupRepository>();
            commGroupRepository.Setup(comm => comm.GetCommissionGroupList(param)).Returns(commissionGroupResultEntity);
            var service = CreateCommissionGroupService(commGroupRepository.Object);

            // Action
            var result = service.GetCommissionGroupList(param);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public static void CreateCommissionGroup_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new CreateUpdateCommissionGroupParam();
            var expectedResult = new ResultInfo();
            var commGroupCommonService = new Mock<ICommGroupCommonService>();
            commGroupCommonService.Setup(comm => comm.CreateCommissionGroup(param)).Returns(expectedResult);
            var service = CreateCommissionGroupService(commGroupCommonService: commGroupCommonService.Object);

            // Action
            var result = service.CreateCommissionGroup(param);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public static void UpdateCommissionGroup_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new CreateUpdateCommissionGroupParam();
            var expectedResult = new ResultInfo();
            var commGroupCommonService = new Mock<ICommGroupCommonService>();
            commGroupCommonService.Setup(comm => comm.UpdateCommissionGroup(param)).Returns(expectedResult);
            var service = CreateCommissionGroupService(commGroupCommonService: commGroupCommonService.Object);

            // Action
            var result = service.UpdateCommissionGroup(param);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public static void DeleteCommissionGroup_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new DeleteCommissionGroupParam();
            var expectedResult = new ResultInfo();
            var commGroupRepository = new Mock<ICommissionGroupRepository>();
            commGroupRepository.Setup(comm => comm.DeleteCommissionGroup(param)).Returns(expectedResult);
            var service = CreateCommissionGroupService(commGroupRepository.Object);

            // Action
            var result = service.DeleteCommissionGroup(param);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        private static CommissionGroupService CreateCommissionGroupService(
            ICommissionGroupRepository commGroupRepository = null,
            ICommGroupCommonService commGroupCommonService = null)
        {
            var fakeCommissionGroupRepository = commGroupRepository ?? Mock.Of<ICommissionGroupRepository>();
            var fakeCommGroupCommonService = commGroupCommonService ?? Mock.Of<ICommGroupCommonService>();

            return new CommissionGroupService(fakeCommissionGroupRepository, fakeCommGroupCommonService);
        }
    }
}
