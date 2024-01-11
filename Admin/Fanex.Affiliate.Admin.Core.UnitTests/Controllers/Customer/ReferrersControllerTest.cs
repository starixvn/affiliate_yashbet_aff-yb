using Fanex.Affiliate.Admin.Core.Controllers.UserPanel;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Controllers.Customer
{
    public class ReferrersControllerTest
    {
        [Fact]
        public void GetReferrers_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new ReferrersParam();
            ReferrersParam passedParam = null;
            var ReferrersRepository = new Mock<IReferrersRepository>();
            ReferrersRepository.Setup(t => t.GetReferrersList(param)).Returns(new List<Referrers> { new Referrers() }).Callback<ReferrersParam>(p => passedParam = p);
            var controller = CreateReferrersController(ReferrersRepository: ReferrersRepository.Object);

            // Action
            var result = controller.GetReferrersList(param) as JsonNetResult;
            var entityResult = result.Data as IEnumerable<Referrers>;

            // Assert
            Assert.Single(entityResult);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetReferrers_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateReferrersController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetReferrersList(new ReferrersParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetReferrersDetailList_WhenCallWithValidParam_ExcuteWithCorrectData()
        {
            // Arrange
            var param = new ReferrersDetailParam();
            ReferrersDetailParam passedParam = null;
            var ReferrersRepository = new Mock<IReferrersRepository>();
            ReferrersRepository.Setup(t => t.GetReferrersDetailList(param)).Returns(new EntityResult<Referrers> { MainData = new List<Referrers> { new Referrers() }, TotalItem = 1 }).Callback<ReferrersDetailParam>(p => passedParam = p);
            var controller = CreateReferrersController(ReferrersRepository: ReferrersRepository.Object);

            // Action
            var result = controller.GetReferrersDetailList(param) as JsonNetResult;
            var entityResult = result.Data as EntityResult<Referrers>;

            // Assert
            Assert.Single(entityResult.MainData);
            Assert.Equal(1, entityResult.TotalItem);
            Assert.Equal(param, passedParam);
        }

        [Fact]
        public void GetReferrersDetailList_WhenCallWithInvalidParam_ReturnEmptyData()
        {
            // Arrange
            var controller = CreateReferrersController();
            controller.ModelState.AddModelError("Error01", string.Empty);

            // Action
            var result = controller.GetReferrersDetailList(new ReferrersDetailParam()) as JsonNetResult;

            // Assert
            Assert.Null(result.Data);
        }

        private ReferrersController CreateReferrersController(
            ISessionService sessionService = null,
            IReferrersRepository ReferrersRepository = null
            )
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeReferrersRepository = ReferrersRepository ?? Mock.Of<IReferrersRepository>();

            return new ReferrersController(fakeSessionService, fakeReferrersRepository);
        }
    }
}