using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.UserPanel
{
    public interface IReferrersRepository
    {
        IEnumerable<Referrers> GetReferrersList(ReferrersParam param);

        EntityResult<Referrers> GetReferrersDetailList(ReferrersDetailParam param);
    }
}
