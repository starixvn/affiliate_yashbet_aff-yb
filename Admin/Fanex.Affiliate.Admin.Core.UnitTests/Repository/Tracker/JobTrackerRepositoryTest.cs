using Fanex.Affiliate.Admin.Core.DTO.Tracker;
using Fanex.Affiliate.Admin.Core.Repository.Tracker;
using Fanex.Data;
using Moq;
using System;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository.Tracker
{
    public class JobTrackerRepositoryTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void InsertDataTracker_WhenCall_InsertSuccessfully(bool isDBNull)
        {
            // Arrange
            var spKey = "Task_API_PushDataTracker_Import";
            var repository = new Mock<JobTrackerRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            objectDb.Setup(obj => obj.GetParameterValue("PushTrackerId")).Returns(isDBNull ? DBNull.Value : 123 as object);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var (Id, Result) = repository.Object.InsertDataTracker(new InsertDataTrackerDTO());

            // Assert
            Assert.Equal(isDBNull ? 0 : 123, Id);
            Assert.True(Result.IsSuccessful);
        }

        [Fact]
        public void UpdateDataTracker_WhenCall_UpdateSuccessfully()
        {
            // Arrange
            var spKey = "Task_API_PushDataTracker_Upd";
            var repository = new Mock<JobTrackerRepository> { CallBase = true };
            var objectDb = new Mock<IObjectDb>();
            objectDb.Setup(obj => obj.ExecuteNonQuery(null, null)).Returns(0);
            objectDb.Setup(obj => obj.GetParameterValue("ErrCode")).Returns(DBNull.Value);
            objectDb.Setup(obj => obj.GetParameterValue("ErrMsg")).Returns(string.Empty);
            repository.Setup(re => re.GetObjectDb(spKey, 120)).Returns(objectDb.Object);

            // Action
            var result = repository.Object.UpdateDataTracker(new UpdateDataTrackerDTO());

            // Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
