using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.UserPanel
{
    public class CustomerMemberService : BaseRepository, ICustomerMemberService
    {
        public EntityResult<CustomerMember> GetCustomerMember(CustomerMemberParam param)
        {
            const string SpKey = "Aff_Admin_Member_Sel";
            var result = new EntityResult<CustomerMember>();
            var dataList = new List<CustomerMember>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignParam(objectDb, param);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new CustomerMember
                        {
                            UserName = Convert.ToString(reader["UserName"]),
                            FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["FirstName"]),
                            LastName = reader["LastName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["LastName"]),
                            Email = (param.CanViewMemberEmail && reader["Email"] != DBNull.Value) ? Convert.ToString(reader["Email"]) : string.Empty,
                            CurrencyName = reader["Currency"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Currency"]),
                            RegisteredDate = reader["RegisteredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["RegisteredDate"])
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        private static void AssignParam(IObjectDb objectDb, CustomerMemberParam param)
        {
            objectDb.SetParameterValue("AffId", param.AffId);
            objectDb.SetParameterValue("MemberType", param.MemberType);
            objectDb.SetParameterValue("FromDate", param.FromDate);
            objectDb.SetParameterValue("ToDate", param.ToDate);
            objectDb.SetParameterValue("IsGetAll", param.IsGetAll);
            objectDb.SetParameterValue("PageSize", param.PageSize);
            objectDb.SetParameterValue("PageIndex", param.PageIndex);
            objectDb.SetParameterValue("NRows", DefaultValue.Int);
        }
    }
}