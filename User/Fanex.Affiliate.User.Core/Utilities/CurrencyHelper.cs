using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class CurrencyHelper
    {
        public static IEnumerable<SelectListItem> GetCurrencyList(IEnumerable<Currency> currencyList, int defaultCurrency)
        {
            var dataList = currencyList.Select(l => new SelectListItem
            {
                Value = l.ExchangeId.ToString(),
                Text = l.Code,
                Selected = l.ExchangeId == defaultCurrency
            });

            return dataList;
        }
    }
}
