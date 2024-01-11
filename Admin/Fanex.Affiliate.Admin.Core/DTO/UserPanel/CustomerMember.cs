using System;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    public class CustomerMember
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string CurrencyName { get; set; }

        public DateTime? RegisteredDate { get; set; }

        public string RegisteredDateString => RegisteredDate?.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
