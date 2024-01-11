using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.MetaModels.Member;

namespace Fanex.Affiliate.Admin.Core.Services.Member
{
    public interface IMemberService
    {
        EntityResult<MemberInfo> GetMemberList(MemberParam param);

        PlayerActionInfo GetMemberDetail(int memberId);

        ResultInfo UpdateMember(UpdateMemberParam param);
    }
}
