using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Domain
{
    public interface IDomainControlService
    {
        IEnumerable<DomainControlInfo> GetDomainControlList(string market);

        ResultInfo InsertDomainControl(InsertUpdateDomainControlParam param);

        ResultInfo UpdateDomainControl(InsertUpdateDomainControlParam param);

        ResultInfo DeleteDomainControl(DeleteDomainControlParam param);
    }
}
