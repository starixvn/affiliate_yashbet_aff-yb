using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.Repository.Member;
using Fanex.Affiliate.Admin.Core.Services.Member;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Member
{
    public class ActiveMemberServiceTest
    {
        [Fact]
        public void GetAllMembers_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var userNames = "abc";
            string passedParam = null;
            var activeMemberRepository = new Mock<IActiveMemberRepository>();
            activeMemberRepository.Setup(t => t.GetAllMembers(userNames)).Returns(new AllMemberEntities
            {
                ActivedMemeberList = new List<ActiveMemberInfo> { new ActiveMemberInfo() },
                InactiveMemberList = new List<InactiveMemberInfo> { new InactiveMemberInfo() },
                MemberNotExist = ""
            }).Callback<string>(p => passedParam = p);
            var service = CreateActiveMemberService(activeMemberRepository.Object);

            // Action
            var result = service.GetAllMembers(userNames);

            // Assert
            Assert.Single(result.ActivedMemeberList);
            Assert.Single(result.InactiveMemberList);
            Assert.Equal("", result.MemberNotExist);
            Assert.Equal(userNames, passedParam);
        }

        private static ActiveMemberService CreateActiveMemberService(IActiveMemberRepository activeMemberRepository = null)
        {
            var fakeActiveMemberRepository = activeMemberRepository ?? Mock.Of<IActiveMemberRepository>();

            return new ActiveMemberService(fakeActiveMemberRepository);
        }
    }
}
