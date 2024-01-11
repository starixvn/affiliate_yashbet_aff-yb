using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Affiliate;
using Fanex.Affiliate.Admin.Core.MetaModels.Affiliate;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.Affiliate
{
    public class AffiliateInfoRepository : BaseRepository, IAffiliateInfoRepository
    {
        public EntityResult<AffiliateInfo> GetAffiliateList(AffiliateInfoParam param)
        {
            const string SpKey = "Aff_Admin_Affiliate_Sel";
            var result = new EntityResult<AffiliateInfo>();
            var dataList = new List<AffiliateInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignAffiliateInfoParam(objectDb, param);
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildAffiliateInfoItem(reader));
                    }
                }
                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }
            return result;
        }

        public ResultInfo ToggleFavorite(FavoriteParam param)
        {
            const string SpKey = "Aff_Admin_Affiliate_Favorite_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignFavoriteParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo ChangePassword(PasswordParam param)
        {
            const string SpKey = "Aff_Admin_Affiliate_Password_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignPasswordParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignAffiliateInfoParam(IObjectDb objectDb, AffiliateInfoParam param)
        {
            objectDb.SetParameterValue("AffCode", param.AffCode);
            objectDb.SetParameterValue("UserId", param.UserId);
            objectDb.SetParameterValue("GroupIds", param.GroupIds);
            objectDb.SetParameterValue("PreferredLanguage", param.Language);
            objectDb.SetParameterValue("Status", param.Status);
            objectDb.SetParameterValue("ParentAffName", param.ParentAffName);
            objectDb.SetParameterValue("UserName", param.UserName);
            objectDb.SetParameterValue("Player_UserName", param.AffPlayer);
            objectDb.SetParameterValue("Email", param.Email);
            objectDb.SetParameterValue("SortType", param.SortBy);
            objectDb.SetParameterValue("SortDirectionType", param.SortDirectionType);
            objectDb.SetParameterValue("PageIndex", param.PageIndex);
            objectDb.SetParameterValue("PageSize", param.PageSize);
            objectDb.SetParameterValue("IsExportExcel", param.IsExportExcel);

            objectDb.SetParameterValue("NRows", DefaultValue.Int);
        }
        private static AffiliateInfo BuildAffiliateInfoItem(IDataReader reader)
        {
            return new AffiliateInfo
            {
                AffId = reader.ToInt(Field.AffId),
                AffCode = reader.ToInt("AffCode"),
                UserName = reader.ToString("Username"),
                Status = reader.ToString("Status"),
                FirstName = reader.ToString("FirstName"),
                LastName = reader.ToString("LastName"),
                Language = reader.ToString("PreferredLanguage"),
                Email = reader.ToString("Email"),
                RegistrationDate = reader.ToDateTime("RegisteredDate"),
                ApprovalDate = reader["ApprovedDate"] == DBNull.Value ? (DateTime?)null : reader.ToDateTime("ApprovedDate"),
                BalanceBf = reader["BFF"] == DBNull.Value ? (decimal?)null : reader.ToDecimal("BFF"),
                Member = reader["NoOfMember"] == DBNull.Value ? (int?)null : reader.ToInt("NoOfMember"),
                CommissionGroup = reader["GroupName"] == DBNull.Value ? "N/A" : reader.ToString("GroupName"),
                IsFavorite = Convert.ToBoolean(reader["IsFavorite"]),
                IsClosed = reader.ToBool("IsClosed"),
                Currency = reader.ToString("Currency"),
                PlayerCurrency = reader.ToString("PlayerCurrency"),
                Phone = reader.ToString("Phone"),
                PlayerUserName = reader.ToString("Player_Username"),
                IsUsingPrivateCommGroup = (reader["IsPrivate"] != DBNull.Value) && reader.ToBool("IsPrivate"),
                ParentAffId = reader["ParentAffId"] == DBNull.Value ? (int?)null : reader.ToInt("ParentAffId"),
                ParentAffName = reader.ToString("ParentAffName"),
                NoOfSubAff = reader["NoOfSubAff"] == DBNull.Value ? (int?)null : reader.ToInt("NoOfSubAff"),
                ApprovedBy = reader.ToString("ApprovedBy")
            };
        }

        private static void AssignFavoriteParam(IObjectDb objectDb, FavoriteParam param)
        {
            objectDb.SetParameterValue("UserId", param.UserId);
            objectDb.SetParameterValue(Field.AffId, param.AffId);
            objectDb.SetParameterValue("IsFavorite", param.IsFavorite);
            objectDb.SetParameterValue("IP", param.Ip);
        }

        private static void AssignPasswordParam(IObjectDb objectDb, PasswordParam param)
        {
            objectDb.SetParameterValue("UserId", param.UserId);
            objectDb.SetParameterValue(Field.AffId, param.AffId);
            objectDb.SetParameterValue("Password", param.Password);
            objectDb.SetParameterValue("IP", param.Ip);
        }
    }
}
