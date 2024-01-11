using System;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class StatementHistory
    {
        public DateTime MonthDate { get; set; }

        public int TransType { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public byte? CalculatedType { get; set; }
    }
}