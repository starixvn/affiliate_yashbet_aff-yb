using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.MediaGallery;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.MediaGallery;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Services.MediaGallery
{
    public class TextListService : BaseRepository, ITextListService
    {
        public EntityResult<TextInfo> GetTextList(TextParam param)
        {
            const string SpKey = "Aff_Admin_Text_Sel";
            var result = new EntityResult<TextInfo>();
            var dataList = new List<TextInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("category", param.Category);
                objectDb.SetParameterValue("language", param.Language);
                objectDb.SetParameterValue("currency", param.Currency);
                objectDb.SetParameterValue("devicetype", param.DeviceType);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);
                objectDb.SetParameterValue(Field.TargetId, param.UserType);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildTextInfoItem(reader));
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public TextInfo GetTextDetail(int selectedTextId, byte userType)
        {
            const string SpKey = "Aff_Admin_Text_Sel";
            TextInfo item = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("textid", selectedTextId);
                objectDb.SetParameterValue(Field.TargetId, userType);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        item = BuildTextInfoItem(reader);
                    }
                }
            }

            return item;
        }

        public ResultInfo InsertText(InsertUpdateTextParam param)
        {
            const string SpKey = "Aff_Admin_Text_Ins";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateText(InsertUpdateTextParam param)
        {
            const string SpKey = "Aff_Admin_Text_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("textid", param.TextId);
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteTexts(DeleteTextParam param)
        {
            const string SpKey = "Aff_Admin_Text_Del";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("textids", param.TextIdList);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);
                objectDb.SetParameterValue(Field.TargetId, param.UserType);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignParam(IObjectDb objectDb, InsertUpdateTextParam param)
        {
            objectDb.SetParameterValue("message", param.TextContent);
            objectDb.SetParameterValue("category", param.Category);
            objectDb.SetParameterValue("language", param.Language);
            objectDb.SetParameterValue("currency", param.Currency);
            objectDb.SetParameterValue("devicetype", param.DeviceType);
            objectDb.SetParameterValue("UserID", param.UserId);
            objectDb.SetParameterValue("IP", param.Ip);
            objectDb.SetParameterValue(Field.TargetId, param.UserType);

            AssignResultOutputParam(objectDb);
        }

        private static TextInfo BuildTextInfoItem(IDataReader reader)
        {
            return new TextInfo
            {
                TextId = Convert.ToInt32(reader["TextId"]),
                TextContent = Convert.ToString(reader["Message"]),
                Category = Convert.ToString(reader["Category"]),
                Language = Convert.ToString(reader["Language"]),
                Currency = Convert.ToString(reader["Currency"]),
                DeviceType = reader.GetVal<byte>("DeviceType"),
                UserType = reader.GetVal<byte>("TargetId")
            };
        }
    }
}
