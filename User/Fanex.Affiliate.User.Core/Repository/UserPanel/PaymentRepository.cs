using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IObjectDBBuilder _builder;

        public PaymentRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public IEnumerable<Payment> GetPaymentList(PaymentParam paymentParam)
        {
            const string uSpKey = "Aff_User_PaymentRequest_Sel";
            var dbMode = paymentParam.IsInternal ? DBMode.View : DBMode.Main;

            var paymentList = new List<Payment>();

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                AssignPaymentParam(objectDb, paymentParam);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        paymentList.Add(BuildPaymentItem(reader));
                    }
                }

                var balBroughtForward = objectDb.GetParameterValue("BalBroughtForward");
                var preferredPayment = objectDb.GetParameterValue("PreferredPayment");
                var minAmount = objectDb.GetParameterValue("MinAmount");

                paymentParam.PreferredPayment = preferredPayment == DBNull.Value ? string.Empty : Convert.ToString(preferredPayment);
                paymentParam.BalBroughtForward = balBroughtForward == DBNull.Value ? 0m : Convert.ToDecimal(balBroughtForward);
                paymentParam.MinAmount = minAmount == DBNull.Value ? 0m : Convert.ToDecimal(minAmount);
            }

            return paymentList;
        }

        public ResultInfo CancelPaymentRequest(PaymentCancelParam cancelParam)
        {
            const string uSpKey = "Aff_User_PaymentRequest_Upd";

            using (var objectDb = _builder.Build(uSpKey))
            {
                objectDb.ExecuteNonQuery(cancelParam);
            }

            return new ResultInfo
            {
                IsOk = cancelParam.ErrCode == 0,
                Message = cancelParam.ErrMsg
            };
        }

        public ResultInfo CreatePaymentRequest(PaymentRequestInfo paymentCreationParam)
        {
            const string uSpKey = "Aff_User_PaymentRequest_Ins";

            using (var objectDb = _builder.Build(uSpKey))
            {
                objectDb.ExecuteNonQuery(paymentCreationParam);
            }

            return new ResultInfo
            {
                IsOk = paymentCreationParam.ErrCode == 0,
                Message = paymentCreationParam.ErrMsg
            };
        }

        private static void AssignPaymentParam(IObjectDb objectDb, PaymentParam param)
        {
            objectDb.SetParameterValue("AffID", param.AffId);
            objectDb.SetParameterValue("Year", param.Year);

            objectDb.SetParameterValue("PreferredPayment", DefaultValue.String);
            objectDb.SetParameterValue("BalBroughtForward", DefaultValue.Decimal);
            objectDb.SetParameterValue("MinAmount", DefaultValue.Decimal);
        }

        private static Payment BuildPaymentItem(IDataReader reader)
        {
            return new Payment
            {
                TransId = reader.ToLong("TransID"),
                RefId = reader.ToLong("RefID"),
                TransDate = reader.ToDateTime("TransDate"),
                CompletedDate = reader.ToDateTime("CompletedDate"),
                Status = reader.ToString("Status"),
                Amount = reader.ToDecimal("Amount"),
                AffDesc = reader.ToString("AffDesc")
            };
        }

        public (IEnumerable<PaymentMethodInfo> PaymentMethods, decimal MinAmount) GetPaymentMethods()
        {
            const string uSpKey = "Aff_PaymentMethod_Sel";
            using (var objectDb = _builder.Build(uSpKey))
            {
                var paymentMethods = objectDb.Query<PaymentMethodInfo>();
                var minAmountParam = objectDb.GetParameterValue("MinAmount");
                var minAmount = minAmountParam == null ? 0 : Convert.ToDecimal(minAmountParam);
                return (paymentMethods, minAmount);
            }
        }
    }
}