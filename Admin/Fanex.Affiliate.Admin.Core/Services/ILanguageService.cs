using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services
{
    public interface ILanguageService
    {
        void SetCulture(string languageCode);

        IEnumerable<Language> GetLanguageList();
    }
}
