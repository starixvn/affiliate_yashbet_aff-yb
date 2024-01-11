namespace Fanex.Affiliate.Link.Services.Interface
{
    /// <summary>
    /// Tracking system statistics.
    /// </summary>
    public interface IGeneralAffiliateInfoService
    {
        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Success or fail.</returns>
        (int affId, string affCode) GetGeneralAffiliateInfo(int? affId = null, string affCode = null);
    }
}
