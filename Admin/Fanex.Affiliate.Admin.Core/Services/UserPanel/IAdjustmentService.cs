using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public interface IAdjustmentService
    {
        decimal GetBalanceBroughtForward(int affId);

        ResultInfo InsertAdjustment(AdjustmentParam param);
    }
}
