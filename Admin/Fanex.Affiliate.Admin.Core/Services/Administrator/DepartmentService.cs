using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Administrator;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.Services.Administrator
{
    public class DepartmentService : BaseRepository, IDepartmentService
    {
        public IEnumerable<DepartmentInfo> GetDepartmentList()
        {
            const string SpKey = "Aff_Con_GroupInfoSel";

            var result = new List<DepartmentInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildDepartmentInfoItem(reader));
                    }
                }
            }

            return result;
        }

        public DepartmentInfo GetDepartmentDetail(int selectedDepartmentId)
        {
            const string SpKey = "Aff_Con_GroupInfoSel";

            DepartmentInfo result = null;

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.GroupId, selectedDepartmentId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = BuildDepartmentInfoItem(reader);
                    }
                }
            }

            return result;
        }

        public ResultInfo InsertDepartment(InsertUpdateDepartmentParam param)
        {
            const string SpKey = "Aff_Con_GroupInfoIns";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.GroupId, DefaultValue.Int);
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateDepartment(InsertUpdateDepartmentParam param)
        {
            const string SpKey = "Aff_Con_GroupInfoUpd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.GroupId, param.DepartmentId);

                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteDepartment(DeleteDepartmentParam param)
        {
            const string SpKey = "Aff_Con_GroupInfoDel";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.GroupId, param.DepartmentId);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public IEnumerable<PolicyInfo> GetRolePolicy(int departmentId)
        {
            const string SpKey = "Aff_Con_GroupPolicySel";

            var result = new List<PolicyInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.GroupId, departmentId);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PolicyInfo
                        {
                            ParentId = reader.GetVal<int>("ParentId"),
                            Policy = Convert.ToInt32(reader["Policy"]),
                            FunctionId = reader.GetVal<int>("FuncId"),
                            FunctionDescription = Convert.ToString(reader["FuncDesc"])
                        });
                    }
                }
            }

            return result.Where(p => !p.FunctionDescription.Contains("Transfer Balance For Player")); //Hide Transfer Player
        }

        public ResultInfo UpdateRolePolicy(UpdatePolicyParam param)
        {
            const string SpKey = "Aff_Con_GroupPolicyUpd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("grouppolicys", param.GroupPolicies);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static DepartmentInfo BuildDepartmentInfoItem(IDataReader reader)
        {
            return new DepartmentInfo
            {
                DepartmentId = Convert.ToInt32(reader["groupid"]),
                DepartmentName = Convert.ToString(reader["groupname"]),
                Description = Convert.ToString(reader["groupdesc"]),
                NoOfUser = reader.GetVal<int>("nouser")
            };
        }

        private static void AssignParam(IObjectDb objectDb, InsertUpdateDepartmentParam param)
        {
            objectDb.SetParameterValue("GroupName", param.DepartmentName);
            objectDb.SetParameterValue("GroupDesc", param.Description);
            objectDb.SetParameterValue("UserID", param.UserId);
            objectDb.SetParameterValue("IP", param.Ip);

            AssignResultOutputParam(objectDb);
        }
    }
}
