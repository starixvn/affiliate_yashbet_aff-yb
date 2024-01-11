namespace Fanex.Affiliate.Admin.Core.DTO.Customer
{
    public class CustomerStatistics
    {
        public int NewSignUpByMonth { get; set; }

        public int NewFdmByMonth { get; set; }

        public int NewSignUpTotal { get; set; }

        public int NewFdmTotal { get; set; }

        public int ActivePlayerByMonth { get; set; }

        public decimal DepositByMonth { get; set; }

        public decimal DepositTotal { get; set; }

        public decimal TurnoverByMonth { get; set; }

        public decimal TurnoverTotal { get; set; }

        public decimal GgrByMonth { get; set; }

        public decimal GgrTotal { get; set; }

        public int UniqueClickPcLaptopByMonth { get; set; }

        public int TotalClickPcLaptopByMonth { get; set; }

        public int UniqueClickMobileByMonth { get; set; }

        public int TotalClickMobileByMonth { get; set; }

        public int NoOfSubAffByMonth { get; set; }

        public int NoOfSubAffTotal { get; set; }
    }
}
