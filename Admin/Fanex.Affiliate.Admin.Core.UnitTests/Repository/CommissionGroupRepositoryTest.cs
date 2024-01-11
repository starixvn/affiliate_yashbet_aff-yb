using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Fanex.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository
{
    public class CommissionGroupRepositoryTest
    {
        private const string SpKeyCommGroupSel = "Aff_Admin_CommGroup_Sel";
        private const string SpKeyCommGroupIns = "Aff_Admin_CommGroup_Ins";
        private const string SpKeyCommGroupUpd = "Aff_Admin_CommGroup_Upd";
        private const string SpKeyCommGroupDel = "Aff_Admin_CommGroup_Del";
        private const string SpKeyDeductionsSel = "Aff_Admin_CommGroup_Deductions_Sel";
        private const string SpKeyCommGroupNormalSel = "Aff_Admin_CommGroupNormal_Sel";
        private const string SpKeyCommGroupAssign = "Aff_Admin_CommGroup_Assign";

        [Fact]
        public void GetCommissionSettingList_WhenCall_ReturnCorrectCommGroupData()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedCommGroup());
            objectDb.Setup(obj => obj.ExecuteReader(It.IsAny<GetCommissionGroupParam>(), null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetCommissionGroupList(new GetCommissionGroupParam());
            var commGroupItem = result.CommGroupList.FirstOrDefault();

            // Assert
            Assert.Single(result.CommGroupList);
            Assert.Empty(result.TierSettingList);
            Assert.Empty(result.TierDeductionSettingList);
            Assert.Empty(result.P2pDeductionSettingList);
            Assert.Equal("07/11/2018 00:00:00", commGroupItem.CreatedDateText);
        }

        [Fact]
        public void GetCommissionSettingList_WhenCall_ReturnCorrectTierSetting()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedTierSetting());
            objectDb.Setup(obj => obj.ExecuteReader(It.IsAny<GetCommissionGroupParam>(), null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetCommissionGroupList(new GetCommissionGroupParam());

            // Assert
            Assert.Empty(result.CommGroupList);
            Assert.Single(result.TierSettingList);
            Assert.Empty(result.TierDeductionSettingList);
            Assert.Empty(result.P2pDeductionSettingList);
        }

        [Fact]
        public void GetCommissionSettingList_WhenCall_ReturnCorrectDeductionSetting()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedDeductionSetting());
            objectDb.Setup(obj => obj.ExecuteReader(It.IsAny<GetCommissionGroupParam>(), null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetCommissionGroupList(new GetCommissionGroupParam());

            // Assert
            Assert.Empty(result.CommGroupList);
            Assert.Empty(result.TierSettingList);
            Assert.Single(result.TierDeductionSettingList);
            Assert.Empty(result.P2pDeductionSettingList);
        }

        [Fact]
        public void GetCommissionSettingList_WhenCall_ReturnCorrectP2pDeductionSetting()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false).Returns(false).Returns(false).Returns(true);
            dataReader.SetupSequence(d => d.NextResult()).Returns(true).Returns(true).Returns(true).Returns(false);

            ReaderHelper.MapReaderData(dataReader, GetExpectedP2pDeductionSetting());
            objectDb.Setup(obj => obj.ExecuteReader(It.IsAny<GetCommissionGroupParam>(), null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetCommissionGroupList(new GetCommissionGroupParam());

            // Assert
            Assert.Empty(result.CommGroupList);
            Assert.Empty(result.TierSettingList);
            Assert.Empty(result.TierDeductionSettingList);
            Assert.Single(result.P2pDeductionSettingList);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            objectDb.Setup(obj => obj.GetParameterValue("GroupId")).Returns(DBNull.Value);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupIns, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.CreateCommissionGroup(new CreateUpdateCommissionGroupParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void CreateCommissionGroup_WhenCall_ExcuteUnsucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("Create Commission Group is failed!");
            objectDb.Setup(obj => obj.GetParameterValue("GroupId")).Returns(1);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupIns, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.CreateCommissionGroup(new CreateUpdateCommissionGroupParam());

            // Assert
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupUpd, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateCommissionGroup(new CreateUpdateCommissionGroupParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void UpdateCommissionGroup_WhenCall_ExcuteUnsucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("Update Commission Group is failed!");
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupUpd, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateCommissionGroup(new CreateUpdateCommissionGroupParam());

            // Assert
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void DeleteCommissionGroup_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupDel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.DeleteCommissionGroup(new DeleteCommissionGroupParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DeleteCommissionGroup_WhenCall_ExcuteUnsucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("Delete Commission Group is failed!");
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupDel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.DeleteCommissionGroup(new DeleteCommissionGroupParam());

            // Assert
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void AssignGroup_WhenCall_ExcuteSucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupAssign, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.AssignGroup(new AssignGroupParam());

            // Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void AssignGroup_WhenCall_ExcuteUnsucceed()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();

            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(123);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns("Assign Commission Group is failed!");
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupAssign, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.AssignGroup(new AssignGroupParam());

            // Assert
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void GetDeductionList_WhenCall_CallCorrectMethodOfObjectDb()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            repository.Setup(re => re.GetObjectDb(SpKeyDeductionsSel, It.IsAny<int>())).Returns(objectDb.Object);

            // Action
            repository.Object.GetDeductionList();

            // Assert
            objectDb.Verify(obj => obj.Query<DeductionItem>(null, null), Times.Once);
        }

        [Fact]
        public void GetNormalCommGroupList_WhenCall_ReturnCorrectNomalCommGroupList()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            var param = new GetGroupListParam();
            var expectedValue = GetExpectedNomalCommGroup();
            ReaderHelper.MapReaderData(dataReader, expectedValue);
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("GroupId")).Returns(expectedValue["GroupId"]);
            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupNormalSel, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetNormalCommGroupList(param);

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedValue["GroupId"], param.GroupId);
        }

        [Fact]
        public void GetNormalCommGroupList_WhenCall_ReturnEmptyList()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(false);

            var param = new GetGroupListParam();
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            objectDb.Setup(obj => obj.GetParameterValue("GroupId")).Returns(DBNull.Value);

            repository.Setup(re => re.GetObjectDb(SpKeyCommGroupNormalSel, 120)).Returns(objectDb.Object);

            //// Action
            repository.Object.GetNormalCommGroupList(param);

            // Assert
            Assert.Equal(0, param.GroupId);
        }

        [Fact]
        public void GetAllCommGroups_WhenCall_ReturnCorrectCommGroupList()
        {
            // Arrange
            var repository = new Mock<CommissionGroupRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(d => d.Read()).Returns(true).Returns(false);

            var expectedValue = GetExpectedNomalCommGroup();
            ReaderHelper.MapReaderData(dataReader, expectedValue);
            objectDb.Setup(obj => obj.ExecuteReader(null, null)).Returns(dataReader.Object);
            repository.Setup(re => re.GetObjectDb("Aff_CommGroup_Sel", 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.GetAllCommGroups();

            // Assert
            Assert.Single(result);
        }

        private IDictionary<string, object> GetExpectedCommGroup()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["RowIdx"] = 1,
                ["GroupId"] = 101,
                ["GroupName"] = "Default",
                ["CreatedDate"] = new DateTime(2018, 7, 11),
                ["Creator"] = "Admin",
                ["Description"] = "Description",
                ["QMTurnover"] = 100.50m,
                ["NextQMTurnover"] = 150.50m,
                ["NoAffs"] = 2
            };
        }

        private IDictionary<string, object> GetExpectedTierSetting()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["GroupId"] = 101,
                ["TierId"] = 1,
                ["MinAmount"] = 0m,
                ["MaxAmount"] = 100m,
                ["MinAp"] = 0,
                ["MaxAp"] = 50,
                ["Commission"] = 0.2m
            };
        }

        private IDictionary<string, object> GetExpectedDeductionSetting()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["GroupId"] = 101,
                ["DeductionId"] = 1,
                ["DeductionName"] = "DeductionName",
                ["ProductIds"] = "1,2,3",
                ["DeductedPercentage"] = 0.7m,
                ["IsDeductedPercent"] = true,
                ["DeductedAmount"] = "0"
            };
        }

        private IDictionary<string, object> GetExpectedP2pDeductionSetting()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["GroupId"] = 101,
                ["DeductionId"] = 1,
                ["DeductionName"] = "DeductionName",
                ["ProductIds"] = "1,2,3",
                ["DeductedPercentage"] = 0.7m,
                ["Commission"] = 0.1m
            };
        }

        private IDictionary<string, object> GetExpectedNomalCommGroup()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["GroupId"] = 133,
                ["GroupName"] = "New Group"
            };
        }
    }
}
