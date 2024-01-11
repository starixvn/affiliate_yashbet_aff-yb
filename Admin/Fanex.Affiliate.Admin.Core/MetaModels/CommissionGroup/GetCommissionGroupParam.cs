
namespace Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup
{
    public class GetCommissionGroupParam
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Nrows { get; set; }
    }
}
