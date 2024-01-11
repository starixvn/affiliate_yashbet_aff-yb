using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Message;
using Fanex.Affiliate.Admin.Core.MetaModels.Message;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Message
{
    public class MessageService : BaseRepository, IMessageService
    {
        public EntityResult<MessageInfo> GetMessageList(MessageParam param)
        {
            const string SpKey = "Aff_Admin_Message_Sel";
            var result = new EntityResult<MessageInfo>();
            var dataList = new List<MessageInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("SearchById", param.SearchById);
                objectDb.SetParameterValue("IsGetAll", param.IsGetAll);
                objectDb.SetParameterValue("FromDate", param.FromDate);
                objectDb.SetParameterValue("ToDate", param.ToDate);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new MessageInfo
                        {
                            MessageId = Convert.ToInt32(reader[Field.MessageId]),
                            Important = Convert.ToBoolean(reader["Important"]),
                            Creator = Convert.ToString(reader["creatorname"]),
                            PostedDate = reader["PostedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["PostedDate"]),
                            UpdatedDate = reader["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDate"]),
                            UpdatedBy = Convert.ToString(reader["UpdatedName"]),
                            Message = Convert.ToString(reader["message_en"])
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public MessageDetail GetMessageDetail(int selectedMessageId)
        {
            const string SpKey = "Aff_Admin_Message_Sel";
            MessageDetail item = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.MessageId, selectedMessageId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        item = new MessageDetail
                        {
                            MessageId = Convert.ToInt32(reader[Field.MessageId]),
                            Important = Convert.ToBoolean(reader["Important"]),
                            MessageContentbn = Convert.ToString(reader["message_bn"]),
                            MessageContenten = Convert.ToString(reader["message_en"]),
                            MessageContentcs = Convert.ToString(reader["message_cn"]),
                            MessageContentth = Convert.ToString(reader["message_th"]),
                            MessageContentvn = Convert.ToString(reader["message_vi"]),
                            MessageContentid = Convert.ToString(reader["message_id"]),
                            MessageContentkh = Convert.ToString(reader["message_kh"]),
                            MessageContenthi = Convert.ToString(reader["message_hi"]),
                            MessageContentms = Convert.ToString(reader["message_ms"]),
                        };
                    }
                }
            }

            return item;
        }

        public ResultInfo InsertMessage(InsertUpdateMessageParam param)
        {
            const string SpKey = "Aff_Admin_Message_Ins";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateMessage(InsertUpdateMessageParam param)
        {
            const string SpKey = "Aff_Admin_Message_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.MessageId, param.MessageId);
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteMessages(DeleteMessageParam param)
        {
            const string SpKey = "Aff_Admin_Message_Del";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("MessageIds", param.MessageIdList);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignParam(IObjectDb objectDb, InsertUpdateMessageParam param)
        {
            objectDb.SetParameterValue("IsImportant", param.Important);
            objectDb.SetParameterValue("msgbn", param.MessageContentbn);
            objectDb.SetParameterValue("msgen", param.MessageContenten);
            objectDb.SetParameterValue("msgcn", param.MessageContentcs);
            objectDb.SetParameterValue("msgth", param.MessageContentth);
            objectDb.SetParameterValue("msgvi", param.MessageContentvn);
            objectDb.SetParameterValue("msgid", param.MessageContentid);
            objectDb.SetParameterValue("msgkh", param.MessageContentkh);
            objectDb.SetParameterValue("msghi", param.MessageContenthi);
            objectDb.SetParameterValue("msgms", param.MessageContentms);
            objectDb.SetParameterValue("UserID", param.UserId);
            objectDb.SetParameterValue("IP", param.Ip);

            AssignResultOutputParam(objectDb);
        }
    }
}
