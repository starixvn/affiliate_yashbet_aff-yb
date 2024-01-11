using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.Report
{
    public class PlayerGgrReportRepository : BaseRepository, IPlayerGgrReportRepository
    {
        public EntityResult<PlayerGgrReport> GetPlayerGgrReportData(PlayerGgrReportParam param)
        {
            const string SpKey = "Aff_Admin_PlayerReport_GGR";
            var result = new EntityResult<PlayerGgrReport>();
            var dataList = new List<PlayerGgrReport>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    var selectedProductIdList = param.ProductIds.Split(',');

                    while (reader.Read())
                    {
                        dataList.Add(BuildDataItem(reader, selectedProductIdList));
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        result.Total = BuildTotalDataItem(reader, selectedProductIdList);
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static void AssignCommonParam(IObjectDb objectDb, PlayerGgrReportParam param)
        {
            objectDb.SetParameterValue("AffUsername", param.AffName);
            objectDb.SetParameterValue("Username", param.PlayerName);
            objectDb.SetParameterValue("FromDate", param.FromDate);
            objectDb.SetParameterValue("ToDate", param.ToDate);
            objectDb.SetParameterValue("ProductIds", param.ProductIds);
            objectDb.SetParameterValue("PageIndex", param.PageIndex);
            objectDb.SetParameterValue("PageSize", param.PageSize);
            objectDb.SetParameterValue("IsExportExcel", param.IsExportExcel);
            objectDb.SetParameterValue("BaseCurrency", param.BaseCurrencyId);

            objectDb.SetParameterValue("NRows", DefaultValue.Int);
        }

        private static PlayerGgrReport BuildDataItem(IDataReader reader, string[] selectedProductIdList)
        {
            var playerGgrReport = new PlayerGgrReport
            {
                AffName = reader.ToString("AffUsername"),
                PlayerName = reader.ToString("Username"),
                RegistrationDate = reader.GetVal<DateTime?>("RegisteredDate"),
                FdmDate = reader.GetVal<DateTime?>("FirstDepositDate"),
                TotalTurnover = reader.ToDecimal("Total_Turnover"),
                TotalGgr = reader.ToDecimal("Total_Ggr"),
            };

            playerGgrReport.ProductReportList = ProductReportList(reader, selectedProductIdList);

            return playerGgrReport;
        }

        private static PlayerGgrReport BuildTotalDataItem(IDataReader reader, string[] selectedProductIdList)
        {
            var playerGgrReport = new PlayerGgrReport
            {
                TotalTurnover = reader.ToDecimal("Total_Turnover"),
                TotalGgr = reader.ToDecimal("Total_Ggr"),
            };

            playerGgrReport.ProductReportList = ProductReportList(reader, selectedProductIdList);

            return playerGgrReport;
        }

        private static Dictionary<string, PlayerGgrProductInfo> ProductReportList(IDataReader reader, string[] selectedProductIdList)
        {
            var productReportList = new Dictionary<string, PlayerGgrProductInfo>();

            foreach (var productId in selectedProductIdList)
            {
                var item = new PlayerGgrProductInfo
                {
                    Turnover = reader.GetVal<decimal>($"{productId}_Turnover", 0),
                    Ggr = reader.GetVal<decimal>($"{productId}_Ggr", 0)
                };

                productReportList.Add(productId, item);
            }

            return productReportList;
        }
    }
}
