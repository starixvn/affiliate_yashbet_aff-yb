using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.UserPanel
{
    public class StatementHistoryRepository : BaseRepository, IStatementHistoryRepository
    {
        public EntityResult<StatementHistory> GetStatementHistoryList(AffiliatePagingByDateParam param)
        {
            const string SpKey = "Aff_Admin_StatementHistory_Sel";
            var result = new EntityResult<StatementHistory>();
            var dataList = new List<StatementHistory>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue("FDate", param.FromDate);
                objectDb.SetParameterValue("TDate", param.ToDate);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue(Field.NRows, DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new StatementHistory
                        {
                            TransId = reader.ToLong("TransId"),
                            Date = reader.ToDateTime(Field.MonthDate),
                            TransType = reader.ToInt("TransType"),
                            Amount = reader.ToDecimal("Amount"),
                            Status = reader.ToString("Status"),
                            Creator = reader.ToString("Creator"),
                            Description = reader.ToString("Description"),
                            CalculatedType = reader.GetVal<byte?>("CalculatedType")
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public EntityResult<SubAffiliate> GetSubAffiliate(SubAffiliateParam param)
        {
            const string uSpKey = "Aff_SubAffiliate_Commission_Sel";

            var result = new EntityResult<SubAffiliate>();
            var dataList = new List<SubAffiliate>();

            using (var objectDb = GetObjectDb(uSpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue(Field.MonthDate, param.MonthDate);
                objectDb.SetParameterValue("BaseCurrency", param.CurrencyId);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue(Field.NRows, DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildSubAffilliateItem(reader));
                    }
                    result.MainData = dataList;
                    result.TotalItem = GetTotalRows(objectDb);

                    if (reader.NextResult() && reader.Read())
                    {
                        result.Total = BuildSubAffilliateItem(reader);
                    }
                }
            }

            return result;
        }

        public DailyEarningResult GetDailyEarning(DailyEarningParam param)
        {
            const string uSpKey = "Aff_DailyEarning_Sel";

            var dailyEarningResult = new DailyEarningResult();
            var dataList = new List<DailyEarning>();

            using (var objectDb = GetObjectDb(uSpKey))
            {
                AssignCommonParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildDailyEarningItem(reader));
                    }
                    dailyEarningResult.MainData = dataList;

                    if (reader.NextResult() && reader.Read())
                    {
                        dailyEarningResult.Total = BuildDailyEarningItem(reader);
                    }
                }
            }

            return dailyEarningResult;
        }

        public IEnumerable<DailyEarningDetail> GetDailyEarningDetail(DailyEarningParam param)
        {
            const string SpKey = "Aff_DailyEarning_Detail_Sel";
            var result = new List<DailyEarningDetail>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DailyEarningDetail
                        {
                            CustId = reader.ToInt("CustId"),
                            UserName = reader.ToString("UserName"),
                            IbcUserName = reader.ToString("IBCUsername"),
                            Ggr = reader.ToDecimal("Ggr"),
                            Deposit = reader.ToDecimal("Deposit"),
                            Withdrawal = reader.ToDecimal("Withdrawal"),
                            Bonus = reader.ToDecimal("Bonus"),
                            Rebate = reader.ToDecimal("Rebate")
                        });
                    }
                }
            }

            return result;
        }
        private static void AssignCommonParam(IObjectDb objectDb, DailyEarningParam param)
        {
            objectDb.SetParameterValue(Field.AffId, param.AffId);
            objectDb.SetParameterValue("ProductId", param.ProductId);
            objectDb.SetParameterValue(Field.MonthDate, param.MonthDate);
            objectDb.SetParameterValue("BaseCurrency", param.BaseCurrencyId);
            objectDb.SetParameterValue("Currencies", param.CurrencyIds);
        }

        private static DailyEarning BuildDailyEarningItem(IDataReader reader)
        {
            return new DailyEarning
            {
                WinlostDate = reader.HasColumn("WinlostDate") ? reader.ToDateTime("WinlostDate") : (DateTime?)null,
                Deposit = reader.ToDecimal("Deposit"),
                Withdrawal = reader.ToDecimal("Withdrawal"),
                MemberGgr = reader.ToDecimal("Ggr"),
                Bonus = reader.ToDecimal("Bonus"),
                Rebate = reader.ToDecimal("Rebate")
            };
        }
        private static SubAffiliate BuildSubAffilliateItem(IDataReader reader)
        {
            return new SubAffiliate
            {
                AffId = reader.ToInt(Field.AffId),
                AffName = reader.ToString("UserName"),
                Commission = reader.ToDecimal("Commission"),
                SubAffCommission = reader.ToDecimal("SubAffCommission")
            };
        }
    }
}
