using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public class ViewLogService : BaseRepository, IViewLogService
    {
        public IEnumerable<LogType> GetLogTypeList()
        {
            const string spKey = "Aff_Admin_LogType_Sel";

            using (var objectDb = GetObjectDb(spKey))
            {
                return objectDb.Query<LogType>();
            }
        }

        public EntityResult<ViewLog> GetViewLogList(ViewLogParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateLog_Sel";
            var result = new EntityResult<ViewLog>();
            var dataList = new List<ViewLog>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffID", param.AffId);
                objectDb.SetParameterValue("LogType", param.LogType);
                objectDb.SetParameterValue("FromDate", param.FromDate);
                objectDb.SetParameterValue("ToDate", param.ToDate);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new ViewLog
                        {
                            Id = Convert.ToInt32(reader["LogId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            UpdatedBy = Convert.ToString(reader["UpdatedName"]),
                            Action = Convert.ToString(reader["Action"]),
                            Description = Convert.ToString(reader["Description"]),
                            Ip = Convert.ToString(reader["IP"]),
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }
    }
}
