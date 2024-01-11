namespace Fanex.Affiliate.Link.DataAccess.Impl
{
    using System;
    using System.Data;
    using Fanex.Affiliate.Link.Commands;
    using Fanex.Affiliate.Link.DataAccess.Interface;
    using Fanex.Data;
    using Fanex.Data.SqlServer;

    /// <summary>
    /// Tracking statistics.
    /// </summary>
    public class GeneralAffiliateInfoDataAccess : IGeneralAffiliateInfoDataAccess
    {
        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Success or fail.</returns>
        public (int affId, string affCode) GetGeneralAffiliateInfo(int? affId = null, string affCode = null)
        {
            const string uSpKey = "Aff_Get_GeneralAffInfo";

            using (var objectDb = new SqlObjectDb(uSpKey))
            {
                objectDb.SetParameterValue("AffId", affId);
                objectDb.SetParameterValue("AffCode", affCode);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (Convert.ToInt32(reader["AffId"]), Convert.ToString(reader["AffCode"]));
                    }

                    return (0,string.Empty);
                }
            }
        }
    }
}
