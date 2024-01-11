using Fanex.Affiliate.Admin.Core.DTO.Member;

namespace Fanex.Affiliate.Admin.Core.Repository.Member
{
    public interface IActiveMemberRepository
    {
        AllMemberEntities GetAllMembers(string userNames);
    }
}
