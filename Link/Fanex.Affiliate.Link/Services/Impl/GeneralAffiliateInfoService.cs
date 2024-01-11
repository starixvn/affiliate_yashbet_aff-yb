namespace Fanex.Affiliate.Link.Services.Impl
{
    using Fanex.Affiliate.Link.DataAccess.Impl;
    using Fanex.Affiliate.Link.DataAccess.Interface;
    using Fanex.Affiliate.Link.Services.Interface;

    /// <summary>
    /// Tracking system statistics.
    /// </summary>
    public class GeneralAffiliateInfoService : IGeneralAffiliateInfoService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IGeneralAffiliateInfoDataAccess _generalAffiliateInfoDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralAffiliateInfoDataAccess" /> class.
        /// </summary>
        public GeneralAffiliateInfoService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralAffiliateInfoDataAccess" /> class.
        /// </summary>
        /// <param name="generalAffiliateInfoDataAccess">Account data access.</param>
        private GeneralAffiliateInfoService(IGeneralAffiliateInfoDataAccess generalAffiliateInfoDataAccess)
        {
            _generalAffiliateInfoDataAccess = generalAffiliateInfoDataAccess ?? new GeneralAffiliateInfoDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Success or fail.</returns>
        public (int affId, string affCode) GetGeneralAffiliateInfo(int? affId = null, string affCode = null)
        {
            return _generalAffiliateInfoDataAccess.GetGeneralAffiliateInfo(affId, affCode);
        }
    }
}
