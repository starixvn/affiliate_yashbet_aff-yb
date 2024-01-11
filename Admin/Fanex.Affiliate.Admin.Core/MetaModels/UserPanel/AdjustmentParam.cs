using Fanex.Affiliate.Admin.Core.Constants;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class AdjustmentParam : AffiliateBasicInfoParam
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }
        public bool IsValid(decimal absBalanceBf)
        {
            return Amount != 0 && Amount - absBalanceBf > Money.AllowedMinValue
                    && Amount + absBalanceBf < Money.AllowedMaxValue;
        }
    }
}

