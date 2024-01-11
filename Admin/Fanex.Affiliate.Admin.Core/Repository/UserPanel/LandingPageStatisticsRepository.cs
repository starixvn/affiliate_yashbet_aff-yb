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
    public class LandingPageStatisticsRepository : BaseRepository, ILandingPageStatisticsRepository
    {
        public IEnumerable<LandingPageStatistics> GetLandingPageStatisticsList(LandingPageStatisticsParam param)
        {
            const string SpKey = "Aff_ClickStatisticsSelByPage";
            var result = new List<LandingPageStatistics>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildLandingPageStatisticsItem(reader, true));
                    }
                }
            }

            return result;
        }

        public EntityResult<LandingPageStatistics> GetLandingPageStatisticsDetailList(LandingPageStatisticsDetailParam param)
        {
            const string SpKey = "Aff_ClickStatisticsDetailBySite";
            var result = new EntityResult<LandingPageStatistics>();
            var dataList = new List<LandingPageStatistics>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);

                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildLandingPageStatisticsItem(reader, false));
                    }
                }
                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static void AssignCommonParam(IObjectDb objectDb, LandingPageStatisticsParam param)
        {
            objectDb.SetParameterValue("AffID", param.AffId);
            objectDb.SetParameterValue("FromDate", param.FromDate);
            objectDb.SetParameterValue("ToDate", param.ToDate);
            objectDb.SetParameterValue("SportCode", param.SelectedPage);
            objectDb.SetParameterValue("LanguageCode", param.LanguageCode);
            objectDb.SetParameterValue("DeviceType", param.DeviceType);
        }

        private static LandingPageStatistics BuildLandingPageStatisticsItem(IDataReader reader, bool isGetLandingPageStatisticsList)
        {
            return new LandingPageStatistics
            {
                Page = isGetLandingPageStatisticsList ? (reader.ToString("SportCode")) : string.Empty,
                LanguageCode = isGetLandingPageStatisticsList ? (reader.ToString("LanguageCode")) : string.Empty,
                WebsiteUrl = isGetLandingPageStatisticsList ? string.Empty : (reader.ToString("URLReferrer")),
                TotalClick = reader.ToInt("TotalClick"),
                UniqueClick = reader.ToInt("UniqueClick"),
                Signups = reader.ToInt("Signups"),
                Fdm = reader.ToInt("FDM"),
            };
        }
    }
}
