using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository.Domain;
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
    public class DomainRepositoryTest
    {
        [Fact]
        public void GetDomainList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Sel";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDomainList());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDomainList(spKey);
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Equal("09/10/2020", item.CreatedDateText);
        }

        [Fact]
        public void GetDomainList_WhenCall_ReturnCorrectDataHasNull()
        {
            // Arrange
            var spKey = "Aff_Admin_AiliateDomain_Sel";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDomainListHasNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDomainList(spKey);
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Null(item.CreatedDateText);
        }

        [Fact]
        public void GetDomainList_WhenCall_ReturnNullData()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Sel";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false);

            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(r => r.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetDomainList(spKey);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InsertDomain_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Ins";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.InsertDomain(new InsertUpdateDomainParam(), spKey);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertDomain_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Ins";
            var message = "Insert Domain is failed.";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.InsertDomain(new InsertUpdateDomainParam(), spKey);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateDomain_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Upd";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateDomain(new InsertUpdateDomainParam(), spKey);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateDomain_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Upd";
            var message = "Update Domain is failed.";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateDomain(new InsertUpdateDomainParam(), spKey);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void DeleteDomain_WhenCallWithValidParam_DeleteSuccessfully()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Del";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.DeleteDomain(new DeleteDomainParam(), spKey);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteDomain_WhenCallWithInvalidParam_DeleteIsFailed()
        {
            // Arrange
            var spKey = "Aff_Admin_AffiliateDomain_Del";
            var message = "Delete Domain is failed.";
            var repository = new Mock<DomainRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.DeleteDomain(new DeleteDomainParam(), spKey);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetDomainList()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["DomainId"] = 123,
                ["DomainName"] = "abc.com",
                ["IsSSL"] = true,
                ["Creator"] = "sa",
                ["CreateDate"] = new DateTime(2020, 09, 10)
            };
        }

        private IDictionary<string, object> GetDomainListHasNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["DomainId"] = 123,
                ["DomainName"] = "abc.com",
                ["IsSSL"] = true,
                ["Creator"] = "sa",
                ["CreateDate"] = DBNull.Value
            };
        }
    }
}
