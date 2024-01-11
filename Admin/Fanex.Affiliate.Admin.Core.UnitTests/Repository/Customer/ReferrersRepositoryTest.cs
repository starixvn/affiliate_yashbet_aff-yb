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
    public class ReferrersRepositoryTest
    {
        [Fact]
        public void GetReferrersList_WhenCall_ReturnCorrectEmailInfoList()
        {
            // Arrange
            var repository = new Mock<ReferrersRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedReferrersList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsSelBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetReferrersList(new ReferrersParam());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetReferrersList_WhenCall_ReturnCorrectEmailInfoListHasNull()
        {
            // Arrange
            var repository = new Mock<ReferrersRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedReferrersListHasNull());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsSelBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetReferrersList(new ReferrersParam());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetReferrersList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<ReferrersRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsSelBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetReferrersList(new ReferrersParam());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetReferrersDetailList_WhenCall_ReturnCorrectEmailInfo()
        {
            // Arrange
            var repository = new Mock<ReferrersRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(1);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedReferrersList());
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsDetailBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetReferrersDetailList(new ReferrersDetailParam());

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetReferrersDetailList_WhenCall_ReturnNull()
        {
            // Arrange
            var repository = new Mock<ReferrersRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.GetParameterValue("NRows")).Returns(DBNull.Value);
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_ClickStatisticsDetailBySite", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetReferrersDetailList(new ReferrersDetailParam());

            // Assert
            Assert.Empty(result.MainData);
        }

        private IDictionary<string, object> GetExpectedReferrersList()
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

        private IDictionary<string, object> GetExpectedReferrersListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["DomainReferrer"] = "abc",
                ["TotalClick"] = DBNull.Value,
                ["UniqueClick"] = DBNull.Value,
                ["Signups"] = DBNull.Value,
                ["FDM"] = DBNull.Value,
                ["URLReferrer"] = "abc.com"
            };
        }
    }
}
