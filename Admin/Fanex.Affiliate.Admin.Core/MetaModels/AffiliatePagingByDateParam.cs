using System;

namespace Fanex.Affiliate.Admin.Core.MetaModels
{
    public class AffiliatePagingByDateParam : AffiliatePagingParam
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
