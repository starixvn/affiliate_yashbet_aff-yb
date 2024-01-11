using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class LanguageService : ILanguageService
    {
        private readonly ISessionService _sessionService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IMemoryCacheService _cacheService;

        public LanguageService(ILanguageRepository languageRepository, ISessionService sessionService, IMemoryCacheService cacheService)
        {
            _languageRepository = languageRepository;
            _sessionService = sessionService;
            _cacheService = cacheService;
        }

        public string GetCurrentLanguage()
            => Thread.CurrentThread.CurrentCulture.Name;

        public string GetFavoriteLanguage()
        {
            var preferedUrlLang = HttpContext.Current.Request["lang"]?.Trim();
            var preferedLang = SystemLanguage.SupportedLanguage.FirstOrDefault(i => string.Equals(i.Value, preferedUrlLang, StringComparison.OrdinalIgnoreCase)).Key;

            if (!string.IsNullOrEmpty(preferedLang) && IsValid(preferedLang))
            {
                return preferedLang;
            }

            var langCookie = HttpContext.Current.Request.Cookies["afffavoritelang" + AppSetting.Instance.SiteName]?.Value;

            return IsValid(langCookie) ? langCookie : SystemLanguage.DefaultLanguageCode;
        }

        public IEnumerable<Language> GetLanguageList(bool? isInternal = null)
        {
            var isForInternal = isInternal ?? _sessionService.IsInternal;
            var languageList = _cacheService.Get<IEnumerable<Language>>(CacheKey.LanguageList);

            if (languageList == null)
            {
                languageList = _languageRepository.GetLanguageList(isForInternal);
                _cacheService.Set(CacheKey.LanguageList, languageList);
            }

            return languageList ?? new List<Language>();
        }

        public bool IsValid(string langCode)
            => !string.IsNullOrWhiteSpace(langCode) && GetLanguageList().Any(l => Equals(l.Code, langCode));
        public void SetCulture(string languageCode)
        {

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageCode);

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = CultureInfo.CreateSpecificCulture("en-US").NumberFormat;
        }
    }
}
