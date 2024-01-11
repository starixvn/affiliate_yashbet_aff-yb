using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services
{
    public interface IAffTypeService
    {
        IEnumerable<AffTypeInfo> GetAffTypeList();
    }
}
