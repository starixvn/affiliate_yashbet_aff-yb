using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public class PlayerDetailService : BaseRepository, IPlayerDetailService
    {
        public EntityResult<PlayerDetail> GetPlayerDetailList(PlayerDetailParam param)
        {
            const string SpKey = "Aff_Admin_PlayersDetail_Sel";
            var result = new EntityResult<PlayerDetail>();
            var dataList = new List<PlayerDetail>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignParam(param, objectDb);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(CreatePlayerDetailItem(reader));
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static PlayerDetail CreatePlayerDetailItem(IDataReader reader)
        {
            return new PlayerDetail
            {
                IbcUserName = reader.ToString("IbcUserName"),
                UserName = reader.ToString("UserName"),
                RegisteredDate = reader["RegisteredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["RegisteredDate"]),
                FirstDepositDate = reader["FirstDepositDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FirstDepositDate"]),
                FirstDepositAmount = reader["FirstDepositAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["FirstDepositAmount"]),
                Deposit = reader.ToDecimal("Deposit"),
                Withdrawal = reader.ToDecimal("Withdrawal"),
                DepositFee = reader.ToDecimal("DepositFee"),
                WithdrawalFee = reader.ToDecimal("WithdrawalFee"),
                Bonus = reader.ToDecimal("Bonus"),
                Rebate = reader.ToDecimal("Rebate"),
                Turnover = reader.ToDecimal("Turnover"),
                Ggr = reader.ToDecimal("Ggr")
            };
        }

        private static void AssignParam(PlayerDetailParam param, IObjectDb objectDb)
        {
            objectDb.SetParameterValue("AffId", param.AffId);
            objectDb.SetParameterValue("ProductId", param.ProductId);
            objectDb.SetParameterValue("MonthDate", param.MonthDate);
            objectDb.SetParameterValue("PageIndex", param.PageIndex);
            objectDb.SetParameterValue("PageSize", param.PageSize);
            objectDb.SetParameterValue("IsExportExcel", param.IsExportExcel);
            objectDb.SetParameterValue("IsSortAsc", param.IsSortAsc);
            objectDb.SetParameterValue("SortById", param.SortById);
            objectDb.SetParameterValue("BaseCurrency", param.BaseCurrencyId);
            objectDb.SetParameterValue("Currencies", param.CurrencyIds);

            objectDb.SetParameterValue("NRows", DefaultValue.Int);
        }
    }
}
