using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.Report
{
    public class AffiliateReportRepository : BaseRepository, IAffiliateReportRepository
    {
        private const int PercentageUnit = 100;

        public DateTime GetReportCutOffDate()
        {
            const string SpKey = "Task_Aff_GetSetting";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("SName", "Report_CutOffDate");

                return objectDb.QueryEntity<DateTime>();
            }
        }

        public EntityResult<AffiliateReport> GetMainReportData(AffiliateReportParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateReport_Sel";
            var result = new EntityResult<AffiliateReport>();
            var dataList = new List<AffiliateReport>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                objectDb.SetParameterValue("GroupIds", param.GroupIds);
                objectDb.SetParameterValue("PreferredLanguage", param.PreferredLanguage);
                objectDb.SetParameterValue("PaymentCurrencies", param.PaymentCurrencyIds);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = BuildDataItem(reader);
                        item.AffId = reader.ToInt("AffId");
                        item.UserName = reader.ToString("Username");
                        item.PaymentCurrency = reader.ToString("PaymentCurrency");
                        item.IsMultiGroupProduct = reader.ToBool("IsMultiGroupProduct");

                        dataList.Add(item);
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        result.Total = BuildDataItem(reader);
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public EntityResult<AffiliateReport> GetDetailReportData(AffiliateReportParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateReport_Detail_Sel";
            var result = new EntityResult<AffiliateReport>();

            var dataList = new List<AffiliateReport>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);
                objectDb.SetParameterValue("IsGetAll", param.IsGetAll);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = BuildDataItem(reader);
                        item.MonthDate = reader.ToDateTime("MonthDate");
                        item.Description = reader.ToString("Description");

                        dataList.Add(item);
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        result.Total = BuildDataItem(reader);
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static void AssignCommonParam(IObjectDb objectDb, AffiliateReportParam param)
        {
            objectDb.SetParameterValue("Username", param.UserName);
            objectDb.SetParameterValue("FromDate", param.FromDate);
            objectDb.SetParameterValue("ToDate", param.ToDate);
            objectDb.SetParameterValue("ProductIds", param.ProductIds);
            objectDb.SetParameterValue("SortType", param.SortType);
            objectDb.SetParameterValue("SortDirectionType", param.SortDirectionType);
            objectDb.SetParameterValue("PageIndex", param.PageIndex);
            objectDb.SetParameterValue("PageSize", param.PageSize);
            objectDb.SetParameterValue("IsExportExcel", param.IsExportExcel);
            objectDb.SetParameterValue("BaseCurrency", param.BaseCurrencyId);
            objectDb.SetParameterValue("Currencies", param.CurrencyIds);
            objectDb.SetParameterValue("NRows", DefaultValue.Int);
        }

        private static AffiliateReport BuildDataItem(IDataReader reader)
        {
            return new AffiliateReport
            {
                TotalClick = reader.ToInt("TotalClick"),
                UniqueClick = reader.ToInt("UniqueClick"),
                Signup = reader.ToInt("Signup"),
                Fdm = reader.ToInt("FDM"),
                ActiveMember = reader.ToInt("ActivePlayer"),
                Deposit = reader.ToDecimal("Deposit"),
                Withdrawal = reader.ToDecimal("Withdrawal"),
                Turnover = reader.ToDecimal("Turnover"),
                Ggr = reader.ToDecimal("Ggr"),
                Bonus = reader.ToDecimal("Bonus"),
                Rebate = reader.ToDecimal("Rebate"),
                PlatformFee = reader.ToDecimal("PlatformFee"),
                DepositFee = reader.ToDecimal("DepositFee"),
                WithdrawalFee = reader.ToDecimal("WithdrawalFee"),
                TotalFee = reader.ToDecimal("TotalFee"),
                CommissionRate = PercentageUnit * reader.ToDecimal("CommissionRate"),
                Earning = reader.ToDecimal("Earning"),
                AffWithdrawal = reader.ToDecimal("AffWithdrawal"),
                AffAdjustment = reader.ToDecimal("AffAdjustment"),
                BalanceBf = reader.ToDecimal("BFF"),
                NoOfSubAff = reader.ToInt("NoOfSubAff"),
                SubAffEarning = reader.ToDecimal("SubAffEarning"),
                P2pEarning = reader.ToDecimal("P2PEarning")
            };
        }
    }
}