using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public interface ICustomerMemberService
    {
        EntityResult<CustomerMember> GetCustomerMember(CustomerMemberParam param);
    }
}
