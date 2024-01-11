using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Domain
{
    public interface IDomainService
    {
        IEnumerable<DomainInfo> GetDomainList(bool isAffDomain);

        ResultInfo InsertDomain(InsertUpdateDomainParam param);

        ResultInfo UpdateDomain(InsertUpdateDomainParam param);

        ResultInfo DeleteDomain(DeleteDomainParam param);
    }
}
