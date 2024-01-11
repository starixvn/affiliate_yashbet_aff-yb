using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Services.Administrator;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Administrator
{
    public class AdministratorServiceTest
    {
        [Fact]
        public void GetUserList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetUser());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_AdminSelByGroupID", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetUserList(0, "", "");
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Equal("11/06/2019 00:00:00", item.CreatedDateText);
        }

        [Fact]
        public void GetUserList_WhenCallWithHasGroupId_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetUserHasCreateDateIsNull());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_AdminSelByGroupID", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetUserList(123, "", "0-9");
            var item = result.FirstOrDefault();

            // Assert
            Assert.Single(result);
            Assert.Null(item.CreatedDateText);
        }

        [Fact]
        public void GetUserDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetUser());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_AdminSelByCustID", 120)).Returns(objectDb.Object);

            // Action
            var data = service.Object.GetUserDetail(123);

            // Assert
            Assert.Equal(123, data.UserId);
        }

        [Fact]
        public void GetUserDetail_WhenCallWithInvalidParam_ReturnNull()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetUser());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_AdminSelByCustID", 120)).Returns(objectDb.Object);

            // Action
            var data = service.Object.GetUserDetail(123456);

            // Assert
            Assert.Null(data);
        }

        [Fact]
        public void InsertUser_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminIns", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertUser(new InsertUserParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertUser_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var message = "Insert user is failed.";
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminIns", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertUser(new InsertUserParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void InsertExistedUser_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminInsFromExisted", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertExistedUser(new InsertExistedUserParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertExistedUser_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Invalid user name.";
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminInsFromExisted", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertExistedUser(new InsertExistedUserParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void IUpdateUser_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateUser(new UpdateUserParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateUser_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Invalid account id.";
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateUser(new UpdateUserParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeStatus_WhenCallWithValidParam_DeleteSuccessfully()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminUpdClosed", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.ChangeStatus(new ChangeStatusParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ChangeStatus_WhenCallWithInvalidParam_DeleteIsFailed()
        {
            // Arrange
            var message = "Delete Department is failed.";
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminUpdClosed", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.ChangeStatus(new ChangeStatusParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void GetAccountPolicyList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetRolePolicy());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_AdminPolicySelByCustID", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetAccountPolicyList(123);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetPolicyListByGroupIds_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetRolePolicy());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_Batch_AdminPolicySel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetPolicyListByGroupIds("123");

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void UpdateAccountPolicy_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminPolicyUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateAccountPolicy(new UpdatePolicyParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateAccountPolicy_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update account policy is failed.";
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_AdminPolicyUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateAccountPolicy(new UpdatePolicyParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangePassword_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Admin_ChangePassword", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.ChangePassword(new UpdatePasswordParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ChangePassword_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update account policy is failed.";
            var service = new Mock<AdministratorService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Admin_ChangePassword", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.ChangePassword(new UpdatePasswordParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetUser()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["custid"] = 123,
                ["username"] = "abc",
                ["FirstName"] = "abc",
                ["LastName"] = "abc",
                ["Creator"] = "sa",
                ["GroupId"] = 234,
                ["GroupName"] = "abc",
                ["username"] = "abc",
                ["CreateDate"] = new DateTime(2019, 11, 6),
                ["closed"] = false
            };
        }

        private IDictionary<string, object> GetUserHasCreateDateIsNull()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["custid"] = 123,
                ["username"] = "abc",
                ["FirstName"] = "abc",
                ["LastName"] = "abc",
                ["Creator"] = "sa",
                ["GroupId"] = 234,
                ["GroupName"] = "abc",
                ["username"] = "abc",
                ["CreateDate"] = DBNull.Value,
                ["closed"] = false
            };
        }

        private IDictionary<string, object> GetRolePolicy()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["ParentId"] = 24,
                ["Policy"] = 1,
                ["FuncId"] = 123,
                ["FuncDesc"] = "abc",
                ["GPolicy"] = 3
            };
        }
    }
}
