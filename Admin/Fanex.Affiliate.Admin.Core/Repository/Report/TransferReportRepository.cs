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
    public class TransferReportRepository : BaseRepository, ITransferReportRepository
    {
        public EntityResult<TransferReport> GetTransferReportData(TransferReportParam param)
        {
            const string SpKey = "Aff_TransferBalanceHistory_Sel";
            var result = new EntityResult<TransferReport>();
            var dataList = new List<TransferReport>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(BuildDataItem(reader));
                    }
                }

                result.MainData = dataList;
                var totalBasedAmount = objectDb.GetParameterValue("TotalBasedAmount");

                result.TotalItem = GetTotalRows(objectDb);
                result.Total = totalBasedAmount == DBNull.Value ? null : new TransferReport
                {
                    BasedAmount = Convert.ToDecimal(totalBasedAmount)
                };
            }

            return result;
        }

        private static void AssignCommonParam(IObjectDb objectDb, TransferReportParam param)
        {
            objectDb.SetParameterValue("TransId", param.TransId);
            objectDb.SetParameterValue("AffName", param.AffName);
            objectDb.SetParameterValue("PlayerName", param.PlayerName);
            objectDb.SetParameterValue("FromDate", param.FromDate);
            objectDb.SetParameterValue("ToDate", param.ToDate);
            objectDb.SetParameterValue("PageIndex", param.PageIndex);
            objectDb.SetParameterValue("PageSize", param.PageSize);
            objectDb.SetParameterValue("IsExportExcel", param.IsExportExcel);
            objectDb.SetParameterValue("BaseCurrency", param.BaseCurrencyId);

            objectDb.SetParameterValue("NRows", DefaultValue.Int);
            objectDb.SetParameterValue("TotalBasedAmount", DefaultValue.Decimal);
        }

        private static TransferReport BuildDataItem(IDataReader reader)
        {
            return new TransferReport
            {
                TransId = reader.ToInt("TransId"),
                CreatedDate = reader.GetVal<DateTime>("TransTime"),
                AffName = reader.ToString("AffName"),
                AffPlayerName = reader.ToString("AffPlayerName"),
                PlayerName = reader.ToString("PlayerName"),
                PlayerCurrency = reader.ToString("Currency"),
                AffPlayerCurrency = reader.ToString("AffPlayerCurrency"),
                Status = reader.ToString("Status"),
                Amount = reader.ToDecimal("Amount"),
                AffPlayerAmount = reader.ToDecimal("AffPlayerAmount"),
                BasedAmount = reader.ToDecimal("BasedAmount"),
            };
        }
    }
}
