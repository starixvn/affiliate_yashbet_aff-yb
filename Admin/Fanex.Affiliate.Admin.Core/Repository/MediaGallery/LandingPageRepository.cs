using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Common.Extensions;
using Fanex.Data;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.MediaGallery
{
    public class LandingPageRepository : BaseRepository, ILandingPageRepository
    {
        public IEnumerable<LandingPage> GetLandingPageList()
        {
            const string SpKey = "Aff_Admin_LandingPage_Sel";
            var result = new List<LandingPage>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new LandingPage
                        {
                            PageId = reader.ToInt(Field.PageId),
                            PageName = reader.ToString("PageName"),
                            PageCode = reader.ToString("PageCode")
                        });
                    }
                }
            }

            return result;
        }

        public LandingPage GetLandingPageById(int pageId)
        {
            const string SpKey = "Aff_Admin_LandingPage_Sel";
            LandingPage result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.PageId, pageId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new LandingPage
                        {
                            PageId = reader.ToInt(Field.PageId),
                            PageName = reader.ToString("PageName"),
                            PageCode = reader.ToString("PageCode")
                        };
                    }
                }
            }

            return result;
        }

        public ResultInfo InsertLandingPage(InsertUpdateLandingPageParam param)
        {
            const string SpKey = "Aff_Admin_LandingPage_Ins";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateLandingPage(InsertUpdateLandingPageParam param)
        {
            const string SpKey = "Aff_Admin_LandingPage_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                objectDb.SetParameterValue(Field.PageId, param.PageId);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteLandingPage(DeleteLandingPageParam param)
        {
            const string SpKey = "Aff_Admin_LandingPage_Del";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("UserId", param.UserId);
                objectDb.SetParameterValue("PageIds", param.PageIds);
                objectDb.SetParameterValue("Ip", param.Ip);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignCommonParam(IObjectDb objectDb, InsertUpdateLandingPageParam param)
        {
            objectDb.SetParameterValue("UserId", param.UserId);
            objectDb.SetParameterValue("PageName", param.PageName);
            objectDb.SetParameterValue("PageCode", param.PageCode);
            objectDb.SetParameterValue("Ip", param.Ip);
        }
    }
}