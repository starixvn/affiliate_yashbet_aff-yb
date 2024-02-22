using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Affiliate.User.Core.Constants
{
    public static class AppContact
    {
        public const string FirstAppContactName = "Skype";

        public static readonly IReadOnlyDictionary<string, string> SecondAppContactList = new Dictionary<string, string>
        {
            [SystemLanguage.EnglishLanguageCode] = "Facebook",
            [SystemLanguage.BangladeshLanguageCode] = "WhatsApp",
            [SystemLanguage.IndiaLanguageCode] = "WhatsApp"
        };
    }
}
