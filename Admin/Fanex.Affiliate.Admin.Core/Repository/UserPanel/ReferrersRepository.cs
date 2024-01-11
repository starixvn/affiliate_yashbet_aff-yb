using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Common.Extensions;
using Fanex.Data;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.UserPanel
{
    public class ReferrersRepository : BaseRepository, IReferrersRepository
    {
        public IEnumerable<Referrers> GetReferrersList(ReferrersParam param)
        {
            const string SpKey = "Aff_ClickStatisticsSelBySite";
            var result = new List<Referrers>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildReferrersItem(reader, true));
                    }
                }
            }

            return result;
        }

        public EntityResult<Referrers> GetReferrersDetailList(ReferrersDetailParam param)
        {
            const string SpKey = "Aff_ClickStatisticsDetailBySite";
            var result = new EntityResult<Referrers>();
            var dataList = new List<Referrers>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                objectDb.SetParameterValue("Domain", param.SelectedSiteUrl);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildReferrersItem(reader, false));
                    }
                }
                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static void AssignCommonParam(IObjectDb objectDb, ReferrersParam param)
        {
            objectDb.SetParameterValue("AffID", param.AffId);
            objectDb.SetParameterValue("FromDate", param.FromDate);
            objectDb.SetParameterValue("ToDate", param.ToDate);
            objectDb.SetParameterValue("DeviceType", param.DeviceType);
        }

        private static Referrers BuildReferrersItem(IDataReader reader, bool isGetReferrersList)
        {
            return new Referrers
            {
                SiteUrl = isGetReferrersList ? (reader.ToString("DomainReferrer")) : string.Empty,
                WebsiteUrl = isGetReferrersList ? string.Empty : (reader.ToString("URLReferrer")),
                TotalClick = reader.ToInt("TotalClick"),
                UniqueClick = reader.ToInt("UniqueClick"),
                Signups = reader.ToInt("Signups"),
                Fdm = reader.ToInt("FDM"),
            };
        }
    }
}
