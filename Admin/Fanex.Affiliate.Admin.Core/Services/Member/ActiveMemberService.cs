using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.Repository.Member;

namespace Fanex.Affiliate.Admin.Core.Services.Member
{
    public class ActiveMemberService : IActiveMemberService
    {
        private readonly IActiveMemberRepository _activeMemberRepository;
        public ActiveMemberService(IActiveMemberRepository activeMemberRepository)
        {
            _activeMemberRepository = activeMemberRepository;
        }

        public AllMemberEntities GetAllMembers(string userNames)
        {
            return _activeMemberRepository.GetAllMembers(userNames);
        }
    }
}
