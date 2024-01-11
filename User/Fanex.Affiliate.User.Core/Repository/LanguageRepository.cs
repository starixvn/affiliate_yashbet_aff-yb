using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Affiliate.User.Core.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IObjectDBBuilder Builder;

        public LanguageRepository(IObjectDBBuilder builder)
        {
            Builder = builder;
        }

        public IEnumerable<Language> GetLanguageList(bool isInternal)
        {
            const string uSpKey = "Aff_Market_Sel";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = Builder.Build(uSpKey, dbMode))
            {
                return objectDb.Query<LanguageType>()
                    .Select(l => new Language
                    {
                        Code = l.MarketId,
                        DisplayName = l.DisplayName,
                        EnglishName = l.MarketName,
                        NativeName = l.MarketNativeName,
                        ShortedCode = l.ShortedCode
                    });
            }
        }
    }
}
