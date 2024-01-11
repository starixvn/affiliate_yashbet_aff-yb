namespace Fanex.Affiliate.Link.DataAccess.Interface
{
    public interface IGeneralAffiliateInfoDataAccess
    {
        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Success or fail.</returns>
        (int affId, string affCode) GetGeneralAffiliateInfo(int? affId = null, string affCode = null);
    }
}
