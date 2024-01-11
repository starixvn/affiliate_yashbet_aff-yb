using Fanex.Affiliate.User.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ILanguageService
    {
        void SetCulture(string languageCode);

        string GetFavoriteLanguage();

        string GetCurrentLanguage();

        bool IsValid(string langCode);

        IEnumerable<Language> GetLanguageList(bool? isInternal = null);
    }
}
