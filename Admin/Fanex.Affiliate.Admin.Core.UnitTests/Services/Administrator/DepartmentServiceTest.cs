using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Services.Administrator;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Administrator
{
    public class DepartmentServiceTest
    {
        [Fact]
        public void GetDepartmentList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDepartment());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_GroupInfoSel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetDepartmentList();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetDepartmentDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDepartment());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_GroupInfoSel", 120)).Returns(objectDb.Object);

            // Action
            var data = service.Object.GetDepartmentDetail(123);

            // Assert
            Assert.Equal(123, data.DepartmentId);
        }

        [Fact]
        public void GetDepartmentDetail_WhenCallWithInvalidParam_ReturnNull()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetDepartment());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_GroupInfoSel", 120)).Returns(objectDb.Object);

            // Action
            var data = service.Object.GetDepartmentDetail(123456);

            // Assert
            Assert.Null(data);
        }

        [Fact]
        public void InsertDepartment_WhenCallWithValidParam_InsertSuccessfully()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupInfoIns", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertDepartment(new InsertUpdateDepartmentParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void InsertDepartment_WhenCallWithInvalidParam_InsertIsFailed()
        {
            // Arrange
            var message = "Insert Department is failed.";
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupInfoIns", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.InsertDepartment(new InsertUpdateDepartmentParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void UpdateDepartment_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupInfoUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateDepartment(new InsertUpdateDepartmentParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateDepartment_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update Department is failed.";
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupInfoUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateDepartment(new InsertUpdateDepartmentParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void DeleteDepartment_WhenCallWithValidParam_DeleteSuccessfully()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupInfoDel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteDepartment(new DeleteDepartmentParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteDepartment_WhenCallWithInvalidParam_DeleteIsFailed()
        {
            // Arrange
            var message = "Delete Department is failed.";
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupInfoDel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.DeleteDepartment(new DeleteDepartmentParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void GetRolePolicy_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetRolePolicy());
            objectDb.Setup(o => o.ExecuteReader(null, null)).Returns(dataReader.Object);
            service.Setup(r => r.GetObjectDb("Aff_Con_GroupPolicySel", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.GetRolePolicy(123);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void UpdateRolePolicy_WhenCallWithValidParam_UpdateSuccessfully()
        {
            // Arrange
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupPolicyUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateRolePolicy(new UpdatePolicyParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateRolePolicy_WhenCallWithInvalidParam_UpdateIsFailed()
        {
            // Arrange
            var message = "Update role policy is failed.";
            var service = new Mock<DepartmentService> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(-999);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(message);
            service.Setup(re => re.GetObjectDb("Aff_Con_GroupPolicyUpd", 120)).Returns(objectDb.Object);

            // Action
            var result = service.Object.UpdateRolePolicy(new UpdatePolicyParam());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(message, result.Message);
        }

        private IDictionary<string, object> GetDepartment()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["groupid"] = 123,
                ["groupname"] = "abc",
                ["groupdesc"] = "abc",
                ["nouser"] = false
            };
        }

        private IDictionary<string, object> GetRolePolicy()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["ParentId"] = 24,
                ["Policy"] = 3,
                ["FuncId"] = 123,
                ["FuncDesc"] = "abc",
            };
        }
    }
}
