using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Payment;
using Fanex.Affiliate.Admin.Core.MetaModels.Payment;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.Payment
{
    public class PaymentRequestRepository : BaseRepository, IPaymentRequestRepository
    {
        public EntityResult<PaymentRequest> GetPaymentRequestList(PaymentRequestParam param)
        {
            const string SpKey = "Aff_Admin_AffiliatePaymentRequest_Sel";
            var result = new EntityResult<PaymentRequest>();
            var dataList = new List<PaymentRequest>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("SearchById", param.SearchBy);
                objectDb.SetParameterValue("Username", param.Username);
                objectDb.SetParameterValue(Field.Status, param.Status);
                objectDb.SetParameterValue("FromDate", param.FromDate);
                objectDb.SetParameterValue("ToDate", param.ToDate);
                objectDb.SetParameterValue("IsGetAll", param.IsGetAll);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new PaymentRequest
                        {
                            TransId = reader.ToLong("Transid"),
                            TransDate = reader.ToDateTime("Transdate"),
                            Status = reader.ToString(Field.Status),
                            AffName = reader.ToString("Username"),
                            Amount = reader.ToDecimal("Amount"),
                            CompletedDate = reader.ToDateTime("Completeddate"),
                            Creator = reader.ToString("Creator")
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public PaymentRequestDetail GetPaymentRequestDetail(long transId)
        {
            const string SpKey = "Aff_Admin_AffiliatePaymentRequest_Detail_Sel";
            PaymentRequestDetail result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("TransID", transId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new PaymentRequestDetail
                        {
                            TransId = reader.ToLong("Transid"),
                            AffId = reader.ToInt("AffId"),
                            AffName = reader.ToString("AffName"),
                            TransDate = reader.ToDateTime("Transdate"),
                            Email = reader.ToString("email"),
                            FirstName = reader.ToString("firstname"),
                            LastName = reader.ToString("lastname"),
                            PaymentMethod = reader.ToString("PreferredPayment"),
                            Amount = reader.ToDecimal("Amount"),
                            TransAmount = reader.ToDecimal("TransAmount"),
                            TransCurrencyCode = reader.ToString("TransCurrencyCode"),
                            Status = reader.ToString(Field.Status),
                            PlayerUsername = reader.ToString("PlayerUsername"),
                            CompletedDate = reader.ToDateTime("Completeddate"),
                            Creator = reader.ToString("Creator"),
                            AdminDesc = reader.ToString("AdminDesc"),
                            AffDesc = reader.ToString("AffDesc"),
                            OriginalTransAmount = reader.ToDecimal("OriginalTransAmount")
                        };
                    }
                }
            }

            return result;
        }

        public ResultInfo UpdatePaymentRequest(PaymentRequestUpdateParam param)
        {
            const string SpKey = "Aff_Admin_AffiliatePaymentRequest_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.Status, param.Status);
                objectDb.SetParameterValue("TransID", param.TransId);
                objectDb.SetParameterValue("AdminDesc", param.AdminDesc);
                objectDb.SetParameterValue("AffDesc", param.AffDesc);
                objectDb.SetParameterValue("UserID", param.UserId);
                objectDb.SetParameterValue("IP", param.Ip);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public (IEnumerable<PaymentMethodInfo> paymentMethods, decimal minAmount) GetPaymentMethod()
        {
            const string SpKey = "Aff_PaymentMethod_Sel";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("MinAmount", DefaultValue.Int);

                var paymentMethods = objectDb.Query<PaymentMethodInfo>();
                var minAmount = objectDb.GetParameterValue("MinAmount");

                var minAmountDecimal = minAmount == DBNull.Value ? 0 : Convert.ToDecimal(minAmount);

                return (paymentMethods, minAmountDecimal);
            }
        }
    }
}
