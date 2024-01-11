using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Domain
{
    public interface ISubAffDomainService
    {
        IEnumerable<DomainInfo> GetSubAffDomainList();

        ResultInfo InsertSubAffDomain(InsertUpdateDomainParam param);

        ResultInfo UpdateSubAffDomain(InsertUpdateDomainParam param);

        ResultInfo DeleteSubAffDomain(DeleteDomainParam param);
    }
}
