using Fanex.Affiliate.Admin.Core.Services.Home;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Home
{
    public class HomeServiceTest
    {
        [Fact]
        public void GetOnlineList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<HomeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetOnlineList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb("Aff_UserOnlineList", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetOnlineList(1, 20);
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Equal("11/15/2019 00:00:00", item.LoggedInDateText);
            Assert.Equal("11/15/2019 00:00:00", item.LastRequestDateText);
        }

        [Fact]
        public void GetOnlineList_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var service = new Mock<HomeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetOnlineListHasNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            service.Setup(r => r.GetObjectDb("Aff_UserOnlineList", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetOnlineList(1, 20);
            var item = result.MainData.FirstOrDefault();

            // Assert
            Assert.Single(result.MainData);
            Assert.Null(item.LoggedInDateText);
            Assert.Null(item.LastRequestDateText);
        }

        [Fact]
        public void GetOnlineList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<HomeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            service.Setup(r => r.GetObjectDb("Aff_UserOnlineList", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetOnlineList(1, 20);

            // Assert
            Assert.Equal(0, result.TotalItem);
        }

        [Fact]
        public void GetGeneralInfo_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<HomeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetGeneralInfo());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_GeneralInfo_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetGeneralInfo();

            // Assert
            Assert.Equal(1, result.PaymentRequestNumber);
        }

        [Fact]
        public void GetGeneralInfo_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<HomeService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_GeneralInfo_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetGeneralInfo();

            // Assert
            Assert.Null(result);
        }

        private IDictionary<string, object> GetOnlineList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["UserName"] = "sa",
                ["LoggedIn"] = new DateTime(2019, 11, 15),
                ["LastRequest"] = new DateTime(2019, 11, 15),
                ["IP"] = "::1",
                ["RoleName"] = "Admin"
            };
        }

        private IDictionary<string, object> GetOnlineListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["UserName"] = "sa",
                ["LoggedIn"] = DBNull.Value,
                ["LastRequest"] = DBNull.Value,
                ["IP"] = "::1",
                ["RoleName"] = "Admin"
            };
        }

        private IDictionary<string, object> GetGeneralInfo()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["PaymentReqNum"] = 1,
                ["BannerNum"] = 2,
                ["PlayerBannerNum"] = 3,
                ["AffiliateBannerNum"] = 4,
                ["AffInProAllNum"] = 1,
                ["PendingAllNum"] = 2,
                ["ApprovedAllNum"] = 3,
                ["DisapprovedAllNum"] = 4,
                ["SuspendedAllNum"] = 4,
                ["DepositMemAllNum"] = 1,
                ["DepositAll"] = 2,
                ["GgrAll"] = 3,
                ["BonusAll"] = 4,
                ["RebateAll"] = 2,
                ["PSPFeeAll"] = 5,
                ["NetWinlostAll"] = 1,
                ["AffInProMonthNum"] = 2,
                ["PendingMonthNum"] = 3,
                ["ApprovedMonthNum"] = 4,
                ["DisapprovedMonthNum"] = 5,
                ["SuspendedMonthNum"] = 5,
                ["DepositMemMonthNum"] = 2,
                ["DepositMonth"] = 3,
                ["GgrMonth"] = 4,
                ["BonusMonth"] = 5,
                ["RebateMonth"] = 3,
                ["PSPFeeMonth"] = 4,
                ["NetWinlostMonth"] = 5,
                ["MemAllNum"] = 4,
                ["NonDepositMemAllNum"] = 3,
                ["MemMonthNum"] = 4,
                ["NonDepositMemMonthNum"] = 3
            };
        }
    }
}
