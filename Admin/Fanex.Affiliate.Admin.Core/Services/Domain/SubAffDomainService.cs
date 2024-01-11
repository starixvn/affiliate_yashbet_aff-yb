using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Affiliate.Admin.Core.Repository.Domain;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Domain
{
    public class SubAffDomainService : BaseRepository, ISubAffDomainService
    {
        private readonly IDomainRepository _domainRepository;

        public SubAffDomainService(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IEnumerable<DomainInfo> GetSubAffDomainList()
        {
            const string SpKey = "Aff_Admin_SubAffiliateDomain_Sel";

            return _domainRepository.GetDomainList(SpKey);
        }

        public ResultInfo InsertSubAffDomain(InsertUpdateDomainParam param)
        {
            const string SpKey = "Aff_Admin_SubAffiliateDomain_Ins";

            return _domainRepository.InsertDomain(param, SpKey);
        }

        public ResultInfo UpdateSubAffDomain(InsertUpdateDomainParam param)
        {
            const string SpKey = "Aff_Admin_SubAffiliateDomain_Upd";

            return _domainRepository.UpdateDomain(param, SpKey);
        }

        public ResultInfo DeleteSubAffDomain(DeleteDomainParam param)
        {
            const string SpKey = "Aff_Admin_SubAffiliateDomain_Del";

            return _domainRepository.DeleteDomain(param, SpKey);
        }
    }
}
