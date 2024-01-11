using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Customer
{
    public class LandingPageStatisticsRepositoryTest
    {
        [Fact]
        public void GetLandingPageStatisticsList_WhenCall_ReturnCorrectEmailInfoList()
        {
            // Arrange
            var repository = new Mock<LandingPageStatisticsRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedLandingPageList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsSelByPage", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageStatisticsList(new LandingPageStatisticsParam());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetLandingPageStatisticsList_WhenCall_ReturnCorrectEmailInfoListHasNull()
        {
            // Arrange
            var repository = new Mock<LandingPageStatisticsRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedLandingPageListHasNull());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsSelByPage", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageStatisticsList(new LandingPageStatisticsParam());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetLandingPageStatisticsList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<LandingPageStatisticsRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsSelByPage", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageStatisticsList(new LandingPageStatisticsParam());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetLandingPageStatisticsDetailList_WhenCall_ReturnCorrectEmailInfo()
        {
            // Arrange
            var repository = new Mock<LandingPageStatisticsRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedLandingPageList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsDetailBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageStatisticsDetailList(new LandingPageStatisticsDetailParam());

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetLandingPageStatisticsDetailList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<LandingPageStatisticsRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsDetailBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetLandingPageStatisticsDetailList(new LandingPageStatisticsDetailParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        private IDictionary<string, object> GetExpectedLandingPageList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["SportCode"] = "abc",
                ["TotalClick"] = 123,
                ["UniqueClick"] = 12,
                ["LanguageCode"] = "vi-VN",
                ["Signups"] = 11,
                ["FDM"] = 1,
                ["URLReferrer"] = "abc.com"
            };
        }

        private IDictionary<string, object> GetExpectedLandingPageListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["SportCode"] = "abc",
                ["TotalClick"] = DBNull.Value,
                ["UniqueClick"] = DBNull.Value,
                ["LanguageCode"] = "vi-VN",
                ["Signups"] = DBNull.Value,
                ["FDM"] = DBNull.Value,
                ["URLReferrer"] = "abc.com"
            };
        }
    }
}
