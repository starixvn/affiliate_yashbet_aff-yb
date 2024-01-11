using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.Customer
{
    public class CustomerLeftPanelRepository : BaseRepository, ICustomerLeftPanelRepository
    {
        public AffiliateInfoOnLeftPanel GetAffiliateInfoOnLeftPanel(string affUserName)
        {
            const string SpKey = "Aff_Admin_AffiliateInfo_Sel";
            AffiliateInfoOnLeftPanel result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("Username", affUserName);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new AffiliateInfoOnLeftPanel
                        {
                            ParentAffId = reader["ParentAffId"] == DBNull.Value ? (int?)null : reader.ToInt("ParentAffId"),
                            ParentAffName = reader.ToString("ParentAffName"),
                            AffId = reader.ToInt(Field.AffId),
                            AffCode = reader.ToString("AffCode"),
                            GroupId = reader.ToInt("GroupId"),
                            AffName = reader.ToString("UserName"),
                            FirstName = reader.ToString("FirstName"),
                            LastName = reader.ToString("LastName"),
                            PreferredLanguage = reader.ToString("PreferredLanguage"),
                            AffType = reader.ToByte("AffType"),
                            BalanceBf = reader.ToDecimal("BalBroughtForward"),
                            CommGroupName = reader.ToString("GroupName"),
                            ContactApps = reader.ToString("ContactApps"),
                            Gender = reader.ToBool("Gender"),
                            IsClosed = reader.ToBool("Closed"),
                            LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : reader.ToDateTime("LastLoginDate"),
                            LastLoginIp = reader.ToString("LastLoginIp"),
                            MoreSiteUrl = reader.ToString("MoreSiteUrls"),
                            SiteUrl = reader.ToString("SiteUrl"),
                            NoSiteDescription = reader.ToString("NoSiteDesc"),
                            Status = reader.ToString("Status"),
                            Phone = reader.ToString("Phone"),
                            IsUsingPrivateCommGroup = (reader["IsPrivate"] != DBNull.Value) && reader.ToBool("IsPrivate"),
                            ApprovedBy = reader.ToString("ApprovedBy"),
                            IsDepositFeePercent = reader.ToBool("Dep_IsDeductedPercent"),
                            IsWithdrawalFeePercent = reader.ToBool("Wit_IsDeductedPercent"),
                            IsDisabledTransfer = reader.ToBool("DisabledTransfer"),
                            PlayerCurrency = reader.ToString("PlayerCurrency"),
                            TransferLimit = reader.ToDecimal("TransferLimit"),
                        };
                    }
                }
            }

            return result;
        }

        public IEnumerable<OnlineList> GetOnlineList(string affUserName)
        {
            const string SpKey = "Aff_Admin_AffiliateOnlineList_Sel";
            var result = new List<OnlineList>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("Username", affUserName);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new OnlineList
                        {
                            Ip = reader.ToString("Ip"),
                            ReloadTime = reader.ToDateTime("ReloadTime")
                        });
                    }
                }
            }

            return result;
        }

        public TransferLimitSettingInfo GetTransferLimitSetting(int affId)
        {
            const string SpKey = "Aff_Admin_TransferLimit_Sel";
            TransferLimitSettingInfo result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, affId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new TransferLimitSettingInfo
                        {
                            PlayerCustId = reader.ToInt("PlayerCustId"),
                            PlayerUsername = reader.ToString("PlayerUsername"),
                            PlayerCurrency = reader.ToString("PlayerCurrency"),
                            TransferLimit = reader.ToDecimal("TransferLimit"),
                            IsDisabledTransfer = reader.ToBool("DisabledTransfer"),
                        };
                    }
                }
            }

            return result;
        }

        public ResultInfo UpdateAffType(UpdateAffTypeParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateAffType_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue("AffType", param.AffType);
                objectDb.SetParameterValue("IP", param.Ip);

                objectDb.SetParameterValue("ErrCode", DefaultValue.Int);

                objectDb.ExecuteNonQuery();

                var code = objectDb.GetParameterValue("ErrCode");

                return new ResultInfo
                {
                    Code = code == DBNull.Value ? 0 : Convert.ToInt32(code),
                };
            }
        }

        public ResultInfo UpdateClosedStatus(UpdateClosedStatusParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateClosed_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue("IsClosed", param.IsClosed);
                objectDb.SetParameterValue("IP", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateLanguage(UpdateLanguageParam param)
        {
            const string SpKey = "Aff_Admin_AffiliatePreferredLanguage_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue("Ip", param.Ip);
                objectDb.SetParameterValue("PreferredLanguage", param.PreferredLanguage);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateStatus(UpdateStatusParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateStatus_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue("Ip", param.Ip);
                objectDb.SetParameterValue("Status", param.Status);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateTransferLimitSetting(TransferLimitSettingParam param)
        {
            const string SpKey = "Aff_Admin_TransferLimit_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue("DisabledTransfer", param.DisabledTransfer);
                objectDb.SetParameterValue("TransferLimit", param.TransferLimit);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }
    }
}
