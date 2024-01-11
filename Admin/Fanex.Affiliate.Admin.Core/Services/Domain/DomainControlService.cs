using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Domain
{
    public class DomainControlService : BaseRepository, IDomainControlService
    {
        public IEnumerable<DomainControlInfo> GetDomainControlList(string market)
        {
            const string SpKey = "Aff_Admin_DomainControl_Sel";

            var result = new List<DomainControlInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("MarketId", market);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DomainControlInfo
                        {
                            DomainId = Convert.ToInt32(reader["DomainControlId"]),
                            MarketId = Convert.ToString(reader["MarketId"]),
                            Market = Convert.ToString(reader["MarketName"]),
                            AffDomainId = reader.GetVal<int>("AffDomainId"),
                            AffDomain = Convert.ToString(reader["AffDomain"]),
                            RedirectDomainId = reader.GetVal<int>("RedirectDomainId"),
                            RedirectDomain = Convert.ToString(reader["RedirectDomain"]),
                            IsUsed = reader.GetVal<bool>("Status"),
                            Creator = Convert.ToString(reader["Creator"]),
                            CreatedDate = reader["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedDate"])
                        });
                    }
                }
            }

            return result;
        }

        public ResultInfo InsertDomainControl(InsertUpdateDomainControlParam param)
        {
            const string SpKey = "Aff_Admin_DomainControl_Ins";

            using (var objectDb = GetObjectDb(SpKey))
            {
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateDomainControl(InsertUpdateDomainControlParam param)
        {
            const string SpKey = "Aff_Admin_DomainControl_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("DomainControlId", param.DomainId);

                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteDomainControl(DeleteDomainControlParam param)
        {
            const string SpKey = "Aff_Admin_DomainControl_Del";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("DomainControlId", param.DomainId);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignParam(IObjectDb objectDb, InsertUpdateDomainControlParam param)
        {
            objectDb.SetParameterValue("AffDomainID", param.AffDomainId);
            objectDb.SetParameterValue("LicenseeDomainId", param.RedirectDomainId);
            objectDb.SetParameterValue("MarketID", param.MarketId);
            objectDb.SetParameterValue("Status", param.IsUsed);
            objectDb.SetParameterValue("UserID", param.UserId);
            objectDb.SetParameterValue("IP", param.Ip);

            AssignResultOutputParam(objectDb);
        }
    }
}
