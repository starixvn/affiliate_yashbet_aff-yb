using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel
{
    public class DailyEarningViewModel
    {
        public IEnumerable<ProductItem> ProductList { get; set; }

        public DailyEarningResult Data { get; set; }

        public int SelectedProduct { get; set; }

        public bool IsKoreanAgentCommGroup { get; set; }
    }
}
