using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository;
using System;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public class AdjustmentService : BaseRepository, IAdjustmentService
    {
        public decimal GetBalanceBroughtForward(int affId)
        {
            const string SpKey = "Aff_CustomerSelBalBF";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffID", affId);

                objectDb.SetParameterValue("BalBroughtForward", DefaultValue.Decimal);

                objectDb.ExecuteNonQuery();

                var balBroughtForward = objectDb.GetParameterValue("BalBroughtForward");
                return balBroughtForward == DBNull.Value ? 0 : Convert.ToDecimal(balBroughtForward);
            }

        }

        public ResultInfo InsertAdjustment(AdjustmentParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateAdjustment_Ins";
            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffID", param.AffId);
                objectDb.SetParameterValue("UserID", param.UserId);
                objectDb.SetParameterValue("Amount", param.Amount);
                objectDb.SetParameterValue("transdesc", param.Description);
                objectDb.SetParameterValue("IP", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }
    }
}
