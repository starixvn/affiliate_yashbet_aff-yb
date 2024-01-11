using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.CommissionGroup;
using Fanex.Affiliate.Admin.Core.MetaModels.CommissionGroup;
using Fanex.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public class CommissionGroupRepository : BaseRepository, ICommissionGroupRepository
    {
        public CommissionGroupResultEntity GetCommissionGroupList(GetCommissionGroupParam param)
        {
            const string SpKey = "Aff_Admin_CommGroup_Sel";

            var result = new CommissionGroupResultEntity();

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader(param))
                {
                    result.CommGroupList = GetCommGroupList(reader);
                    result.TierSettingList = GetTierSettingList(reader);
                    result.TierDeductionSettingList = GetFeeDeductionSettingList(reader);
                    result.P2pDeductionSettingList = GetP2pDeductionSettingList(reader);
                }

                param.Nrows = GetTotalRows(objectDb);
            }

            return result;
        }

        public ResultInfo CreateCommissionGroup(CreateUpdateCommissionGroupParam param)
        {
            const string SpKey = "Aff_Admin_CommGroup_Ins";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignCreateUpdateCommissionGroupParam(objectDb, param);
                objectDb.SetParameterValue("IsPrivate", param.IsPrivate);
                objectDb.SetParameterValue(Field.GroupId, DefaultValue.Int);

                objectDb.ExecuteNonQuery();

                var groupId = objectDb.GetParameterValue(Field.GroupId);
                param.GroupId = groupId == DBNull.Value ? 0 : Convert.ToInt32(groupId);

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateCommissionGroup(CreateUpdateCommissionGroupParam param)
        {
            const string SpKey = "Aff_Admin_CommGroup_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.GroupId, param.GroupId);
                AssignCreateUpdateCommissionGroupParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteCommissionGroup(DeleteCommissionGroupParam param)
        {
            const string SpKey = "Aff_Admin_CommGroup_Del";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue(Field.GroupId, param.GroupId);
                objectDb.SetParameterValue("IP", param.Ip);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public IEnumerable<DeductionItem> GetDeductionList()
        {
            const string SpKey = "Aff_Admin_CommGroup_Deductions_Sel";

            using (var objectDb = GetObjectDb(SpKey))
            {
                return objectDb.Query<DeductionItem>();
            }
        }
        public IEnumerable<SelectItem> GetNormalCommGroupList(GetGroupListParam param)
        {
            const string SpKey = "Aff_Admin_CommGroupNormal_Sel";
            var result = new List<SelectItem>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AffId", param.AffId);
                objectDb.SetParameterValue(Field.GroupId, DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SelectItem
                        {
                            Id = reader.ToString(Field.GroupId),
                            Name = reader.ToString(Field.GroupName)
                        });
                    }
                }
                var groupId = objectDb.GetParameterValue(Field.GroupId);
                param.GroupId = groupId == DBNull.Value ? 0 : Convert.ToInt32(groupId);
            }

            return result;
        }

        public IEnumerable<SelectItem> GetAllCommGroups()
        {
            const string SpKey = "Aff_CommGroup_Sel";
            var result = new List<SelectItem>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SelectItem
                        {
                            Id = reader.ToString(Field.GroupId),
                            Name = reader.ToString(Field.GroupName)
                        });
                    }
                }
            }

            return result;
        }

        public ResultInfo AssignGroup(AssignGroupParam param)
        {
            const string SpKey = "Aff_Admin_CommGroup_Assign";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.UserId, param.UserId);
                objectDb.SetParameterValue("AffId", param.AffId);
                objectDb.SetParameterValue(Field.GroupId, param.GroupId);
                objectDb.SetParameterValue("Ip", param.Ip);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static IEnumerable<CommGroup> GetCommGroupList(IDataReader reader)
        {
            var commGroupList = new List<CommGroup>();
            while (reader.Read())
            {
                commGroupList.Add(new CommGroup
                {
                    RowIdx = reader.ToInt("RowIDX"),
                    GroupId = reader.ToInt(Field.GroupId),
                    GroupName = reader.ToString(Field.GroupName),
                    CreatedDate = reader.ToDateTime("CreatedDate"),
                    Creator = reader.ToString("Creator"),
                    Description = reader.ToString("Description"),
                    NoAffs = reader.ToInt("NoAffs"),
                    SubAffCommRate = reader.ToDecimal("SubCommRate"),
                    IsActivePlayerSum = reader.ToBool("IsActivePlayerSum")
                });
            }

            return commGroupList;
        }

        private static IEnumerable<TierSetting> GetTierSettingList(IDataReader reader)
        {
            var tierSettingList = new List<TierSetting>();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    tierSettingList.Add(new TierSetting
                    {
                        GroupId = reader.ToInt(Field.GroupId),
                        TierId = reader.ToInt("TierId"),
                        MinAmount = reader.ToDecimal("MinAmount"),
                        MinAp = reader.ToInt("MinAm"),
                        Commission = reader.ToDecimal("Commission"),
                        GroupProductId = reader.ToInt("GroupProductId")
                    });
                }
            }

            return tierSettingList;
        }

        private static IEnumerable<DeductionSetting> GetFeeDeductionSettingList(IDataReader reader)
        {
            var deductionSettingList = new List<DeductionSetting>();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    deductionSettingList.Add(new DeductionSetting
                    {
                        GroupId = reader.ToInt(Field.GroupId),
                        DeductionId = reader.ToInt("DeductionId"),
                        DeductionName = reader.ToString("DeductionName"),
                        ProductIds = reader.ToString("ProductIds"),
                        DeductedPercentage = reader.ToDecimal("DeductedPercentage"),
                        DeductedAmount = reader.ToString("DeductedAmount"),
                        IsDeductedPercent = reader.ToBool("IsDeductedPercent"),
                        GroupProductId = reader.ToInt("GroupProductId")
                    });
                }
            }

            return deductionSettingList;
        }

        private static IEnumerable<DeductionSetting> GetP2pDeductionSettingList(IDataReader reader)
        {
            var p2pDeductionSettingList = new List<DeductionSetting>();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    p2pDeductionSettingList.Add(new DeductionSetting
                    {
                        GroupId = reader.ToInt(Field.GroupId),
                        DeductionId = Deduction.P2pDeductionId,
                        DeductionName = Deduction.P2pDeductionName,
                        ProductIds = reader.ToString("ProductIds"),
                        DeductedPercentage = reader.ToDecimal("DeductedPercentage"),
                        Commission = reader.ToDecimal("Commission")
                    });
                }
            }

            return p2pDeductionSettingList;
        }

        private static void AssignCreateUpdateCommissionGroupParam(IObjectDb objectDb, CreateUpdateCommissionGroupParam param)
        {
            objectDb.SetParameterValue(Field.UserId, param.UserId);
            objectDb.SetParameterValue(Field.GroupName, param.GroupName);
            objectDb.SetParameterValue("SubCommRate", param.SubAffCommRate);
            objectDb.SetParameterValue("Description", param.Description);
            objectDb.SetParameterValue("DeductionList", param.TierDeductionsXml);
            objectDb.SetParameterValue("StraightList", param.P2pDeductionsXml);
            objectDb.SetParameterValue("TierList", param.TierSettingXml);
            objectDb.SetParameterValue("IP", param.Ip);
            objectDb.SetParameterValue("IsActivePlayerSum", param.IsActivePlayerSum);

            AssignResultOutputParam(objectDb);
        }
    }
}