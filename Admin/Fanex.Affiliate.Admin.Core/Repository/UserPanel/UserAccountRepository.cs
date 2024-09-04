using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.UserPanel
{
    public class UserAccountRepository : BaseRepository, IUserAccountRepository
    {
        public UserAccount GetUserAccountInfo(int affId)
        {
            const string SpKey = "Aff_Admin_AffiliateInfo_Sel";
            UserAccount result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, affId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new UserAccount
                        {
                            AffId = reader.ToInt(Field.AffId),
                            AffName = reader.ToString("UserName"),
                            FirstName = reader.ToString("FirstName"),
                            LastName = reader.ToString("LastName"),
                            Gender = reader.GetVal<bool?>("Gender"),
                            Email = reader.ToString("Email"),
                            Phone = reader.ToString("Phone"),
                            ContactApps = reader.ToString("ContactApps"),
                            Address = reader.ToString("Address"),
                            City = reader.ToString("City"),
                            State = reader.ToString("State"),
                            Postcode = reader.ToString("Postal"),
                            Country = reader.ToString("Country"),
                            MoreSiteUrls = reader.ToString("MoreSiteUrls"),
                            SiteUrl = reader.ToString("SiteUrl"),
                            PreferredPayment = reader.ToString("PreferredPayment"),
                            PlayerUsername = reader.ToString("PlayerUsername"),
                            NoSiteDescription = reader.ToString("NoSiteDesc"),
                            NetellerAccountId = reader.ToString("NetellerAccountID"),
                            MoneybookersEmail = reader.ToString("MoneybookersEmail"),
                            BankName = reader.ToString("BankName"),
                            BankAddress = reader.ToString("BankAddress"),
                            BankCode = reader.ToString("BankSwiftCode"),
                            BankAccountName = reader.ToString("BankAccountName"),
                            BankAccountNumber = reader.ToString("BankAccountNo"),
                            Question = reader.ToString("Question"),
                            Answer = reader.ToString("Answer"),
                            MailOption = reader.GetVal<bool?>("MailOption")
                        };
                    }
                }
            }

            return result;
        }

        public ResultInfo UpdateUserAccount(UserAccountParam param)
        {
            const string SpKey = "Aff_Admin_AffiliateInfo_Upd";
            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.AffId, param.AffId);
                objectDb.SetParameterValue("UserId", param.UserId);
                objectDb.SetParameterValue("FirstName", param.FirstName);
                objectDb.SetParameterValue("LastName", param.LastName);
                objectDb.SetParameterValue("Gender", param.Gender);
                objectDb.SetParameterValue("Email", param.Email);
                objectDb.SetParameterValue("Phone", param.Phone);

                objectDb.SetParameterValue("ContactApps", param.ContactApps);
                objectDb.SetParameterValue("Address", param.Address);
                objectDb.SetParameterValue("City", param.City);
                objectDb.SetParameterValue("State", param.State);
                objectDb.SetParameterValue("Postal", param.Postcode);
                objectDb.SetParameterValue("Country", param.Country);

                objectDb.SetParameterValue("MailOption", param.MailOption);

                objectDb.SetParameterValue("SiteURL", param.SiteUrl);
                objectDb.SetParameterValue("MoreSiteURLs", param.MoreSiteUrls);
                objectDb.SetParameterValue("NoSiteDesc", param.NoSiteDescription);

                objectDb.SetParameterValue("PreferredPayment", param.PreferredPayment);
                objectDb.SetParameterValue("Player_Username", param.PlayerUsername);
                objectDb.SetParameterValue("NetellerAccountId", param.NetellerAccountId);
                objectDb.SetParameterValue("MoneybookersEmail", param.MoneybookersEmail);
                objectDb.SetParameterValue("BankName", param.BankName);
                objectDb.SetParameterValue("BankAddress", param.BankAddress);
                objectDb.SetParameterValue("BankSwiftCode", param.BankCode);
                objectDb.SetParameterValue("BankAccountName", param.BankAccountName);
                objectDb.SetParameterValue("BankAccountNo", param.BankAccountNumber);
                objectDb.SetParameterValue("IP", param.Ip);

                objectDb.SetParameterValue("Player_CustId", param.Player_CustId);
                objectDb.SetParameterValue("Player_Currency", param.Player_Currency);

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
