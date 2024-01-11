using Affiliate.YB.Core;
using Affiliate.YB.SpParams.Criteria;
using Fanex.Data.Repository;

namespace Affiliate.YB.SpParams.Commands
{
    public abstract class IdenticalSuffixNonQueryCommand : IdenticalSurfixCriteriaBase, INonQueryCommand
    {
        protected IdenticalSuffixNonQueryCommand(IPrefixProvider prefixProvider = null) : base(prefixProvider)
        {
        }
    }
}
