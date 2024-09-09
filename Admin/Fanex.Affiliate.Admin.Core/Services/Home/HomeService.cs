using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Home;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Services.Home
{
    public class HomeService : BaseRepository, IHomeService
    {
        public EntityResult<OnlineListInfo> GetOnlineList(int pageIndex, int pageSize)
        {
            const string SpKey = "Aff_UserOnlineList";

            var result = new EntityResult<OnlineListInfo>();
            var dataList = new List<OnlineListInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("PageIndex", pageIndex);
                objectDb.SetParameterValue("PageSize", pageSize);

                objectDb.SetParameterValue("TotalPages", DefaultValue.Int);
                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new OnlineListInfo
                        {
                            UserName = Convert.ToString(reader["UserName"]),
                            LoggedInDate = reader["LoggedIn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LoggedIn"]),
                            LastRequestDate = reader["LastRequest"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastRequest"]),
                            Ip = Convert.ToString(reader["IP"]),
                            Role = Convert.ToString(reader["RoleName"])
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public GeneralInfo GetGeneralInfo()
        {
            const string SpKey = "Aff_Admin_GeneralInfo_Sel";
            GeneralInfo result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = BuildItem(reader);
                    }
                }
            }

            return result;
        }

        public GeneralInfo BuildItem(IDataReader reader)
        {
            return new GeneralInfo
            {
                PaymentRequestNumber = reader.GetVal<int>("PaymentReqNum"),
                BannerNumber = reader.GetVal<int>("BannerNum"),
                PlayerBannerNum = reader.GetVal<int>("PlayerBannerNum"),
                AffiliateBannerNum = reader.GetVal<int>("AffiliateBannerNum"),
                AllAffiliateNumber = reader.GetVal<int>("AffInProAllNum"),
                PendingAffiliateNumber = reader.GetVal<int>("PendingAllNum"),
                ApprovedAffiliateNumber = reader.GetVal<int>("ApprovedAllNum"),
                DisapprovedAffiliateNumber = reader.GetVal<int>("DisapprovedAllNum"),
                SuspendedAffiliateNumber = reader.GetVal<int>("SuspendedAllNum"),
                AllMemberDepositNumber = reader.GetVal<int>("DepositMemAllNum"),
                AllDepositNumber = reader.GetVal<decimal>("DepositAll"),
                AllGgrNumber = reader.GetVal<decimal>("GgrAll"),
                AllBonusNumber = reader.GetVal<decimal>("BonusAll"),
                AllRebateNumber = reader.GetVal<decimal>("RebateAll"),
                AllPSPFeeNumber = reader.GetVal<decimal>("PSPFeeAll"),
                AllNetWinlostNumber = reader.GetVal<decimal>("NetWinlostAll"),
                AllAffiliateMonthNumber = reader.GetVal<int>("AffInProMonthNum"),
                PendingAffiliateMonthNumber = reader.GetVal<int>("PendingMonthNum"),
                ApprovedAffiliateMonthNumber = reader.GetVal<int>("ApprovedMonthNum"),
                DisapprovedAffiliateMonthNumber = reader.GetVal<int>("DisapprovedMonthNum"),
                SuspendedAffiliateMonthNumber = reader.GetVal<int>("SuspendedMonthNum"),
                AllMemberDepositMonthNumber = reader.GetVal<int>("DepositMemMonthNum"),
                AllDepositMonthNumber = reader.GetVal<decimal>("DepositMonth"),
                AllGgrMonthNumber = reader.GetVal<decimal>("GgrMonth"),
                AllBonusMonthNumber = reader.GetVal<decimal>("BonusMonth"),
                AllRebateMonthNumber = reader.GetVal<decimal>("RebateMonth"),
                AllPSPFeeMonthNumber = reader.GetVal<decimal>("PSPFeeMonth"),
                AllNetWinlostMonthNumber = reader.GetVal<decimal>("NetWinlostMonth"),
                MemAllNum = reader.GetVal<int>("MemAllNum"),
                NonDepositMemAllNum = reader.GetVal<int>("NonDepositMemAllNum"),
                MemMonthNum = reader.GetVal<int>("MemMonthNum"),
                NonDepositMemMonthNum = reader.GetVal<int>("NonDepositMemMonthNum"),
            };
        }
    }
}
