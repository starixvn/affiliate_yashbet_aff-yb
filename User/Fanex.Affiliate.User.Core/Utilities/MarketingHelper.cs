using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class MarketingHelper
    {
        public static IEnumerable<SelectListItem> GetLanguageList(IEnumerable<Language> languageList, string preferredLanguage)
        {
            var dataList = languageList.Select(l => new SelectListItem
            {
                Value = l.Code,
                Text = l.DisplayName,
                Selected = l.Code.MatchWith(preferredLanguage)
            });

            return dataList;
        }
    }
}
