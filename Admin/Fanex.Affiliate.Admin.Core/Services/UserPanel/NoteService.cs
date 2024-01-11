using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public class NoteService : BaseRepository, INoteService
    {
        public EntityResult<NoteDetail> GetNoteList(AffiliatePagingParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateNote_Sel";
            var result = new EntityResult<NoteDetail>();
            var dataList = new List<NoteDetail>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffID", param.AffId);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dataItem = BuildNoteItem(reader);
                        dataItem.CreatedBy = Convert.ToString(reader["UpdatedName"]);
                        dataItem.LastModify = reader["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDate"]);

                        dataList.Add(dataItem);
                    }
                }
                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public Note GetNoteContentById(int affId, int noteId)
        {
            const string SpKey = "Aff_Admin_AffiliateNote_Detail_Sel";
            Note result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffID", affId);
                objectDb.SetParameterValue("NoteId", noteId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = BuildNoteItem(reader);
                    }
                }
            }

            return result;
        }

        public ResultInfo InsertNote(InsertUpdateNoteParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateNote_Ins";
            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateNote(InsertUpdateNoteParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateNote_Upd";
            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                objectDb.SetParameterValue("Noteid", param.NoteId);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignCommonParam(IObjectDb objectDb, InsertUpdateNoteParam param)
        {
            objectDb.SetParameterValue("AffID", param.AffId);
            objectDb.SetParameterValue("UserID", param.UserId);
            objectDb.SetParameterValue("Note", param.NoteContent);
            objectDb.SetParameterValue("IP", param.Ip);
        }

        private static NoteDetail BuildNoteItem(IDataReader reader)
        {
            return new NoteDetail
            {
                NoteId = Convert.ToInt32(reader["NoteId"]),
                NoteContent = Convert.ToString(reader["Note"])
            };
        }
    }
}
