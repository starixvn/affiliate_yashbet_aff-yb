using Fanex.Affiliate.User.Core.DTO.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.ViewModels.UserPanel
{
    public class MessageHistoryViewModel
    {
        public IEnumerable<MessageHistory> MessageList { get; set; }

        public int TotalItems { get; set; }
    }
}
