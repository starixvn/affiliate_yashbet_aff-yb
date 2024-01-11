using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.EmailHistory;
using Fanex.Affiliate.Admin.Core.MetaModels.EmailHistory;
using Fanex.Affiliate.Admin.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Services.EmailHistory
{
    public class EmailHistoryService : BaseRepository, IEmailHistoryService
    {
        public ResultInfo DeleteEmailHistory(DeleteEmailHistoryParam param)
        {
            const string SpKey = "Aff_Admin_EmailHistory_Del";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("MIDs", param.EmailIdList);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public EmailHistoryDetailInfo GetEmailHistoryDetail(int selectedEmailId)
        {
            const string SpKey = "Aff_Admin_EmailHistory_Detail_Sel";

            EmailHistoryDetailInfo result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("MID", selectedEmailId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = BuildDataItem(reader);
                        result.Content = Convert.ToString(reader["Content"]);
                    }
                }
            }

            return result;
        }

        public EntityResult<EmailHistoryInfo> GetEmailHistoryList(EmailHistoryParam param)
        {
            const string SpKey = "Aff_Admin_EmailHistory_Sel";
            var result = new EntityResult<EmailHistoryInfo>();
            var dataList = new List<EmailHistoryInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("SearchById", param.SearchById);
                objectDb.SetParameterValue("IsGetAll", param.IsGetAll);
                objectDb.SetParameterValue("SearchText", param.SearchText);
                objectDb.SetParameterValue("FromDate", param.FromDate);
                objectDb.SetParameterValue("ToDate", param.ToDate);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dataItem = BuildDataItem(reader);
                        dataItem.AffName = Convert.ToString(reader["Affname"]);

                        dataList.Add(dataItem);
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static EmailHistoryDetailInfo BuildDataItem(IDataReader reader)
        {
            return new EmailHistoryDetailInfo
            {
                EmailId = Convert.ToInt32(reader["MID"]),
                Sender = Convert.ToString(reader["Sender"]),
                Email = Convert.ToString(reader["Email"]),
                Subject = Convert.ToString(reader["Subject"]),
                SentDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
