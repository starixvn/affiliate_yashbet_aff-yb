using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel
{
    public class StatementHistoryViewModel
    {
        public DateTime MonthDate { get; set; }

        public int TransType { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public byte? CalculatedType { get; set; }

        public string DateString
        {
            get
            {
                return TransType == Constants.TransType.Payment
                    || TransType == Constants.TransType.Adjustment
                    ? MonthDate.ToString("MM-dd-yyyy")
                    : MonthDate.ToString("MMMM-yyyy");
            }
        }

        public string DisplayStatus
        {
            get
            {
                switch (Status.ToLowerInvariant())
                {
                    case "running":
                    case "pending":
                        return Resources.Lang.Proccessing;

                    case "completed":
                        return Resources.Lang.Approved;

                    case "cancelled":
                        return Resources.Lang.Cancelled;

                    case "approved":
                        return Resources.Lang.Approved;

                    case "rejected":
                        return Resources.Lang.Rejected;

                    case "current":
                        return Resources.Lang.Current;

                    default:
                        return Status;
                }
            }
        }

        public string TransTypeString
        {
            get
            {
                switch (TransType)
                {
                    case Constants.TransType.Adjustment:
                        return Resources.Lang.Adjustment;
                    case Constants.TransType.Commission:
                        return Resources.Lang.MonthlyEarning;
                    case Constants.TransType.Payment:
                        return Resources.Lang.PaymentRequest;
                    case Constants.TransType.SubAffBonus:
                        return Resources.Lang.SubAffiliate;
                    case Constants.TransType.MonthlyTotal:
                        {
                            if (CalculatedType == Constants.CalculatedType.CalculatedMonth || CalculatedType == Constants.CalculatedType.UnCalculatedMonth)
                            {
                                return Resources.Lang.MonthlyTotal;
                            }

                            return Resources.Lang.Transaction;
                        }
                    default:
                        return Resources.Lang.Transaction;
                }
            }
        }

        public string DescriptionDisplay
        {
            get
            {
                switch (TransType)
                {
                    case Constants.TransType.Commission:
                        return GetCommissionDescription(CalculatedType);
                    case Constants.TransType.SubAffBonus:
                        return GetSubAffiliateDescription(CalculatedType);
                    case Constants.TransType.MonthlyTotal:
                        return GetMonthlyTotalDescription(CalculatedType);
                    default:
                        return Description;
                }
            }
        }

        public IEnumerable<StatementHistoryViewModel> Children { get; set; } = new List<StatementHistoryViewModel>();

        private static string GetCommissionDescription(byte? calculatedType)
        {
            switch (calculatedType)
            {
                case Constants.CalculatedType.CalculatedMonth:
                    return Resources.Lang.MonthlyEarning;
                case Constants.CalculatedType.UnCalculatedMonth:
                    return Resources.Lang.UncalculatedMonthlyEarning;
                default:
                    return Resources.Lang.CurrentMonthlyEarning;
            }
        }

        private static string GetSubAffiliateDescription(byte? calculatedType)
        {
            switch (calculatedType)
            {
                case Constants.CalculatedType.CalculatedMonth:
                    return Resources.Lang.Commission;
                case Constants.CalculatedType.UnCalculatedMonth:
                    return Resources.Lang.UncalculatedCommission;
                default:
                    return Resources.Lang.CurrentCommission;
            }
        }

        private static string GetMonthlyTotalDescription(byte? calculatedType)
        {
            switch (calculatedType)
            {
                case Constants.CalculatedType.CalculatedMonth:
                    return Resources.Lang.MonthlyTotal;
                case Constants.CalculatedType.UnCalculatedMonth:
                    return Resources.Lang.UncalculatedMonth;
                default:
                    return Resources.Lang.CurrentMonthStatistics;
            }
        }
    }
}
