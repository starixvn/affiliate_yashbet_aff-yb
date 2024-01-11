namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class CustomerMemberParam : AffiliatePagingByDateParam
    {
        public byte MemberType { get; set; }

        public bool IsGetAll { get; set; }

        public bool CanViewMemberEmail { get; set; }
    }
}
