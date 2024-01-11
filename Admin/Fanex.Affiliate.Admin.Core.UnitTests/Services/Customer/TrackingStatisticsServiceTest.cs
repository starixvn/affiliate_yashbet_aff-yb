using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Customer
{
    public class TrackingStatisticsServiceTest
    {
        [Fact]
        public void GetTrackingCodeList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetTrackingCodeList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelTrackingCode", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingCodeList(1271808);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetTrackingCodeList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelTrackingCode", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingCodeList(1271808);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetTrackingStatisticsList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetTrackingStatisticsList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelClickStatisticsAllMonths", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingStatisticsList(new TrackingStatisticsParam());
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Equal("December-2019", item.MonthText);
            Assert.Equal("12/27/2019", item.DateText);
        }

        [Fact]
        public void GetTrackingStatisticsList_WhenCall_ReturnCorrectDataHasNextResult()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetTrackingStatisticsList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelClickStatisticsAllMonths", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingStatisticsList(new TrackingStatisticsParam());
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Equal("December-2019", item.MonthText);
            Assert.Equal("12/27/2019", item.DateText);
        }

        [Fact]
        public void GetTrackingStatisticsList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelClickStatisticsAllMonths", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingStatisticsList(new TrackingStatisticsParam());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetTrackingStatisticsDetailList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetTrackingStatisticsListHasNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelClickStatisticsMonthDetail", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingStatisticsDetailList(new TrackingStatisticsDetailParam { SelectedMonth = new DateTime(2019, 12, 27) });
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Equal("December-2019", item.MonthText);
            Assert.Equal("12/27/2019", item.DateText);
        }

        [Fact]
        public void GetTrackingStatisticsDetailList_WhenCall_ReturnNull()
        {
            // Arrange
            var service = new Mock<TrackingStatisticsService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_AffiliateSelClickStatisticsMonthDetail", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetTrackingStatisticsDetailList(new TrackingStatisticsDetailParam { SelectedMonth = new DateTime(2019, 12, 27) });

            // Assert
            Assert.Empty(result);
        }

        private IDictionary<string, object> GetTrackingCodeList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TrackingCode"] = "[None]"
            };
        }

        private IDictionary<string, object> GetTrackingStatisticsList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TotalClick"] = 123,
                ["UniqueClick"] = 123,
                ["Date"] = new DateTime(2019, 12, 27),
            };
        }

        private IDictionary<string, object> GetTrackingStatisticsListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["TotalClick"] = DBNull.Value,
                ["UniqueClick"] = DBNull.Value,
                ["Date"] = new DateTime(2019, 12, 27),
            };
        }
    }
}
