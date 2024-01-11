using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.Domain
{
    public interface IDomainRepository
    {
        IEnumerable<DomainInfo> GetDomainList(string spKey);

        ResultInfo InsertDomain(InsertUpdateDomainParam param, string spKey);

        ResultInfo UpdateDomain(InsertUpdateDomainParam param, string spKey);

        ResultInfo DeleteDomain(DeleteDomainParam param, string spKey);
    }
}
