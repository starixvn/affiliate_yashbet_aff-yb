using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.MetaModels.Member;
using Fanex.Affiliate.Admin.Core.Repository.Member;
using Fanex.Affiliate.Admin.Core.Services.Member;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Member
{
    public class MemberServiceTest
    {
        [Fact]
        public void GetMemberList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new MemberParam();
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(comm => comm.GetMemberList(param)).Returns(new EntityResult<MemberInfo> { MainData = new List<MemberInfo> { new MemberInfo() }, TotalItem = 1 });
            var service = CreateMemberService(memberRepository.Object);

            // Action
            var result = service.GetMemberList(param);

            // Assert
            Assert.Single(result.MainData);
        }

        [Fact]
        public void GetMemberDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(comm => comm.GetMemberDetail(It.IsAny<int>())).Returns(new PlayerActionInfo());
            var service = CreateMemberService(memberRepository.Object);

            // Action
            var result = service.GetMemberDetail(123);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateMember_WhenCall_UpdateSuccessfully()
        {
            // Arrange
            var param = new UpdateMemberParam();
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(comm => comm.UpdateMember(param)).Returns(ResultInfo.Success);
            var service = CreateMemberService(memberRepository.Object);

            // Action
            var result = service.UpdateMember(param);

            // Assert
            Assert.True(result.IsSuccessful);
        }

        private static MemberService CreateMemberService(IMemberRepository memberRepository = null)
        {
            var fakeMemberRepository = memberRepository ?? Mock.Of<IMemberRepository>();

            return new MemberService(fakeMemberRepository);
        }
    }
}
