using Fanex.Affiliate.Admin.Core.Constants;

namespace Fanex.Affiliate.Admin.Core.DTO.Customer
{
    public class TransferLimitSettingInfo
    {
        public int PlayerCustId { get; set; }

        public string PlayerUsername { get; set; }

        public string PlayerCurrency { get; set; }

        public decimal TransferLimit { get; set; }

        public bool IsDisabledTransfer { get; set; }

        public bool IsUnlimited => TransferLimit == Money.AllowedMaxValue;
    }
}
