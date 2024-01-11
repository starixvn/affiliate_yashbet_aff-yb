using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Common
{
    public class SessionServiceTest
    {
        [Fact]
        public void PropertyUserId_WhenCall_UserIdCorrectly()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();
            var userId = 123;

            service.UserId = userId;

            Assert.Equal(userId, service.UserId);
        }

        [Fact]
        public void PropertyUserId_WhenCall_ReturnZero()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();

            Assert.Equal(0, service.UserId);
        }

        [Fact]
        public void PropertyOnUserId_WhenCall_OnUserIdCorrectly()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();
            var onUserId = 123;

            service.OnUserId = onUserId;

            Assert.Equal(onUserId, service.OnUserId);
        }

        [Fact]
        public void PropertyOnUserId_WhenCall_ReturnZero()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();

            Assert.Equal(0, service.OnUserId);
        }

        [Fact]
        public void PropertyAffName_WhenCall_AffIdCorrectly()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();
            var affName = "abc";

            service.AffName = affName;

            Assert.Equal(affName, service.AffName);
        }

        [Fact]
        public void PropertyAffName_WhenCall_ReturnEmpty()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();

            Assert.Equal(string.Empty, service.AffName);
        }

        [Fact]
        public void PropertyUserPermissionList_WhenCall_UserPermissionListCorrectly()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();

            service.UserPermissionList = new List<UserPermission> { new UserPermission() };

            Assert.Single(service.UserPermissionList);
        }

        [Fact]
        public void PropertyUserPermissionList_WhenCall_ReturnEmpty()
        {
            MockingHelper.SetUpHttpContextSession();
            var service = CreateService();

            Assert.Empty(service.UserPermissionList);
        }

        private static SessionService CreateService() => new SessionService();
    }
}
