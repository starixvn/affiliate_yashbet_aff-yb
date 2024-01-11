using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository.Domain;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Domain
{
    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;

        public DomainService(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }
        public IEnumerable<DomainInfo> GetDomainList(bool isAffDomain)
        {
            var spKey = isAffDomain ? "Aff_Admin_AffiliateDomain_Sel" : "Aff_Admin_LicenseeDomain_Sel";

            return _domainRepository.GetDomainList(spKey);
        }

        public ResultInfo InsertDomain(InsertUpdateDomainParam param)
        {
            var spKey = param.IsAffDomain ? "Aff_Admin_AffiliateDomain_Ins" : "Aff_Admin_LicenseeDomain_Ins";

            return _domainRepository.InsertDomain(param, spKey);
        }

        public ResultInfo UpdateDomain(InsertUpdateDomainParam param)
        {
            var spKey = param.IsAffDomain ? "Aff_Admin_AffiliateDomain_Upd" : "Aff_Admin_LicenseeDomain_Upd";

            return _domainRepository.UpdateDomain(param, spKey);
        }

        public ResultInfo DeleteDomain(DeleteDomainParam param)
        {
            var spKey = param.IsAffDomain ? "Aff_Admin_AffiliateDomain_Del" : "Aff_Admin_LicenseeDomain_Del";

            return _domainRepository.DeleteDomain(param, spKey);
        }
    }
}
