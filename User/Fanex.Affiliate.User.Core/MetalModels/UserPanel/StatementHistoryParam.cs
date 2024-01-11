namespace Fanex.Affiliate.User.Core.MetalModels.UserPanel
{
    public class StatementHistoryParam : BaseParam
    {
        public int AffId { get; set; }

        public int BaseCurrency { get; set; }

        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }

        public int NRows { get; set; }
    }
}
