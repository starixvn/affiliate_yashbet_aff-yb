namespace Fanex.Affiliate.Link.DataAccess.Impl
{
    using Fanex.Affiliate.Link.Criterias;
    using Fanex.Affiliate.Link.DataAccess.Interface;
    using Fanex.Affiliate.Link.DTOs;
    using Fanex.Data.Repository;

    /// <summary>
    /// Tracking statistics.
    /// </summary>
    public class GeneralAffiliateInfoDataAccess : IGeneralAffiliateInfoDataAccess
    {
        private readonly IDynamicRepository _dynamicRepository;

        public GeneralAffiliateInfoDataAccess()
            : this(null)
        {
        }

        public GeneralAffiliateInfoDataAccess(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Success or fail.</returns>
        public (int affId, string affCode) GetGeneralAffiliateInfo(int? affId = null, string affCode = null)
        {
            var criteria = new GeneralAffiliateInfoCriteria
            {
                AffId = affId,
                AffCode = affCode,
            };

            var affInfo = _dynamicRepository.Get<AffiliateInfo>(criteria);
            return (affInfo.AffId, affInfo.AffCode);
        }
    }
}
