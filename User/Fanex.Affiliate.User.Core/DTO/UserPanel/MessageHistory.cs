using System;

namespace Fanex.Affiliate.User.Core.DTO.UserPanel
{
    public class MessageHistory
    {
        public int MsgId { get; set; }

        public DateTime PostedDate { get; set; }

        public string Message { get; set; }

        public bool Important { get; set; }
    }
}
