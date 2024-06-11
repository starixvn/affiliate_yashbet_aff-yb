using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class CurrencyService : ICurrencyService
    {
        private const int UUS_EXCHANGE_ID = 20;

        private readonly IAppSetting _appSetting;
        private readonly IMemoryCacheService _cacheService;
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(
            IAppSetting appSetting,
            IMemoryCacheService cacheService,
            ICurrencyRepository currencyRepository)
        {
            _appSetting = appSetting;
            _cacheService = cacheService;
            _currencyRepository = currencyRepository;
        }
        public IEnumerable<Currency> GetBaseCurrencyList()
        {
            var currencyList = _cacheService.Get<IEnumerable<Currency>>(CacheKey.BaseCurrencyList);

            if (currencyList == null)
            {
                currencyList = _currencyRepository.GetBaseCurrencyList();

                if (_appSetting.IsShowUusBaseCurrency && !currencyList.Any(c => c.ExchangeId == UUS_EXCHANGE_ID))
                {
                    currencyList = currencyList.Concat(new[] { new Currency { ExchangeId = UUS_EXCHANGE_ID, Code = "UUS" } });
                }

                _cacheService.Set(CacheKey.BaseCurrencyList, currencyList);
            }

            return currencyList ?? new List<Currency>();
        }
    }
}
