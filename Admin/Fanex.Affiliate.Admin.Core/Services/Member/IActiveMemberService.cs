using Fanex.Affiliate.Admin.Core.DTO.Member;

namespace Fanex.Affiliate.Admin.Core.Services.Member
{
    public interface IActiveMemberService
    {
        AllMemberEntities GetAllMembers(string userNames);
    }
}
