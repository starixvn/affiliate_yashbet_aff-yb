using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel
{
    public class MyAccountViewModel
    {
        public AccountInformation AccountInformation { get; set; }

        public IEnumerable<SelectListItem> BaseCurrencyList { get; set; }
    }
}
