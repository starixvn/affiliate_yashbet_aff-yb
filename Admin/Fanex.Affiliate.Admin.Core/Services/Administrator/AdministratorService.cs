using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Administrator;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Services.Administrator
{
    public class AdministratorService : BaseRepository, IAdministratorService
    {
        public IEnumerable<AdministratorInfo> GetUserList(int groupId, string userName, string firstLetter)
        {
            const string SpKey = "Aff_Con_AdminSelByGroupID";

            var result = new List<AdministratorInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                if (groupId != 0)
                {
                    objectDb.SetParameterValue(Field.GroupId, groupId);
                }
                objectDb.SetParameterValue(Field.UserName, "%" + userName + "%");
                objectDb.SetParameterValue("firstchar", firstLetter == "0-9" ? "[" + firstLetter + "]%" : firstLetter + "%");

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dataItem = BuildAdministratorInfo(reader);
                        dataItem.Creator = Convert.ToString(reader["Creator"]);
                        dataItem.IsOpen = !reader.GetVal<bool>("closed");
                        dataItem.GroupName = Convert.ToString(reader["GroupName"]);
                        dataItem.CreatedDate = reader["CreateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreateDate"]);

                        result.Add(dataItem);
                    }
                }
            }

            return result;
        }

        public AdministratorInfo GetUserDetail(int userId)
        {
            const string SpKey = "Aff_Con_AdminSelByCustID";

            AdministratorInfo result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.CustId, userId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = BuildAdministratorInfo(reader);
                    }
                }
            }

            return result;
        }

        public IEnumerable<PolicyInfo> GetAccountPolicyList(int groupId)
        {
            const string SpKey = "Aff_Con_AdminPolicySelByCustID";

            var result = new List<PolicyInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.CustId, groupId);
                objectDb.SetParameterValue(Field.GroupId, DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildPolicyInfo(reader));
                    }
                }
            }

            return result.Where(p => !p.FunctionDescription.Contains("Transfer Balance For Player")); //Hide Transfer Player
        }

        public IEnumerable<PolicyInfo> GetPolicyListByGroupIds(string groupIds)
        {
            const string SpKey = "Aff_Con_Batch_AdminPolicySel";

            var result = new List<PolicyInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("custids", groupIds);
                objectDb.SetParameterValue(Field.GroupId, DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildPolicyInfo(reader));
                    }
                }
            }

            return result.Where(p => !p.FunctionDescription.Contains("Transfer Balance For Player")); //Hide Transfer Player
        }

        public ResultInfo InsertUser(InsertUserParam param)
        {
            const string SpKey = "Aff_Con_AdminIns";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.CustId, DefaultValue.Int);
                objectDb.SetParameterValue(Field.UserName, "AA" + param.UserName);
                objectDb.SetParameterValue("userpwd", param.Password);
                objectDb.SetParameterValue(Field.FirstName, param.FirstName);
                objectDb.SetParameterValue(Field.LastName, param.LastName);
                objectDb.SetParameterValue(Field.GroupId, param.GroupId);

                AssignUserInfoParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo InsertExistedUser(InsertExistedUserParam param)
        {
            const string SpKey = "Aff_Con_AdminInsFromExisted";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.UserName, param.UserName);
                objectDb.SetParameterValue(Field.GroupId, param.GroupId);
                objectDb.SetParameterValue("creator", param.UserId);
                objectDb.SetParameterValue("Ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateUser(UpdateUserParam param)
        {
            const string SpKey = "Aff_Con_AdminUpd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.CustId, param.AccountId);
                objectDb.SetParameterValue(Field.FirstName, param.FirstName);
                objectDb.SetParameterValue(Field.LastName, param.LastName);
                objectDb.SetParameterValue(Field.GroupId, param.GroupId);

                AssignUserInfoParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo ChangeStatus(ChangeStatusParam param)
        {
            const string SpKey = "Aff_Con_AdminUpdClosed";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.CustId, param.SelectedUserId);
                objectDb.SetParameterValue("closed", !param.IsOpen);

                AssignUserInfoParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateAccountPolicy(UpdatePolicyParam param)
        {
            const string SpKey = "Aff_Con_AdminPolicyUpd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("mempolicys", param.GroupPolicies);

                AssignUserInfoParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo ChangePassword(UpdatePasswordParam param)
        {
            const string SpKey = "Aff_Admin_ChangePassword";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.CustId, param.AccountId);
                objectDb.SetParameterValue("NewPwd", param.Password);

                AssignUserInfoParam(objectDb, param);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private AdministratorInfo BuildAdministratorInfo(IDataReader reader)
        {
            return new AdministratorInfo
            {
                UserId = Convert.ToInt32(reader[Field.CustId]),
                UserName = Convert.ToString(reader[Field.UserName]),
                FirstName = Convert.ToString(reader[Field.FirstName]),
                LastName = Convert.ToString(reader[Field.LastName]),
                GroupId = reader.GetVal<int>(Field.GroupId)
            };
        }

        private PolicyInfo BuildPolicyInfo(IDataReader reader)
        {
            return new PolicyInfo
            {
                ParentId = reader.GetVal<int>("ParentId"),
                Policy = Convert.ToInt32(reader["Policy"]),
                FunctionId = Convert.ToInt32(reader["FuncId"]),
                FunctionDescription = Convert.ToString(reader["FuncDesc"]),
                GroupPolicy = reader.GetVal<byte>("GPolicy")
            };
        }

        private static void AssignUserInfoParam(IObjectDb objectDb, UserInfoParam param)
        {
            objectDb.SetParameterValue("UserId", param.UserId);
            objectDb.SetParameterValue("IP", param.Ip);
        }
    }
}
