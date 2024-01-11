using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Services.Domain;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Domain
{
    public class DomainControlServiceTest
    {
        [Fact]
        public void GetDomainControlList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDomainControlList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_DomainControl_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetDomainControlList("en-GB");
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Equal("10/29/2019", item.CreatedDateText);
        }

        [Fact]
        public void GetDomainControlList_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDomainControlListHasNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_DomainControl_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetDomainControlList("en-GB");
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Null(item.CreatedDateText);
        }

        [Fact]
        public void GetDomainControlList_WhenCall_ReturnNullData()
        {
            // Arrange
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Admin_DomainControl_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetDomainControlList(string.Empty);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InsertDomainControl_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_DomainControl_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertDomainControl(new InsertUpdateDomainControlParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertDomainControl_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var message = "Insert Domain is failed.";
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_DomainControl_Ins", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertDomainControl(new InsertUpdateDomainControlParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateDomainControl_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_DomainControl_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateDomainControl(new InsertUpdateDomainControlParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateDomainControl_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update Domain is failed.";
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_DomainControl_Upd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateDomainControl(new InsertUpdateDomainControlParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void DeleteDomainControl_WhenCallWithValidParam_DeleteSuccessfully()
        {
            // Arrange
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_DomainControl_Del", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteDomainControl(new DeleteDomainControlParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteDomainControl_WhenCallWithInvalidParam_DeleteIsFailed()
        {
            // Arrange
            var message = "Delete Domain is failed.";
            var service = new Mock<DomainControlService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_DomainControl_Del", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteDomainControl(new DeleteDomainControlParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetDomainControlList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["DomainControlId"] = 123,
                ["MarketID"] = "en-GB",
                ["MarketName"] = "English (Euro)",
                ["AffDomainID"] = 234,
                ["AffDomain"] = "go.test.com",
                ["RedirectDomainId"] = 345,
                ["RedirectDomain"] = "abc.com",
                ["Status"] = true,
                ["Creator"] = "sa",
                ["CreatedDate"] = new DateTime(2019, 10, 29)
            };
        }

        private IDictionary<string, object> GetDomainControlListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["DomainControlId"] = 123,
                ["MarketID"] = "en-GB",
                ["MarketName"] = "English (Euro)",
                ["AffDomainID"] = 234,
                ["AffDomain"] = "go.test.com",
                ["RedirectDomainId"] = 345,
                ["RedirectDomain"] = "abc.com",
                ["Status"] = true,
                ["Creator"] = "sa",
                ["CreatedDate"] = DBNull.Value
            };
        }
    }
}
