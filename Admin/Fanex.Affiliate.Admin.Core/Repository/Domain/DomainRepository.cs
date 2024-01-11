using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.Domain
{
    public class DomainRepository : BaseRepository, IDomainRepository
    {
        public IEnumerable<DomainInfo> GetDomainList(string spKey)
        {
            var result = new List<DomainInfo>();

            using (var objectDb = GetObjectDb(spKey))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildSubAffDomainInfoItem(reader));
                    }
                }
            }

            return result;
        }

        public ResultInfo InsertDomain(InsertUpdateDomainParam param, string spKey)
        {
            using (var objectDb = GetObjectDb(spKey))
            {
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo UpdateDomain(InsertUpdateDomainParam param, string spKey)
        {
            using (var objectDb = GetObjectDb(spKey))
            {
                objectDb.SetParameterValue("DomainId", param.DomainId);
                AssignParam(objectDb, param);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        public ResultInfo DeleteDomain(DeleteDomainParam param, string spKey)
        {
            using (var objectDb = GetObjectDb(spKey))
            {
                objectDb.SetParameterValue("DomainId", param.DomainId);
                objectDb.SetParameterValue("userid", param.UserId);
                objectDb.SetParameterValue("ip", param.Ip);

                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }

        private static void AssignParam(IObjectDb objectDb, InsertUpdateDomainParam param)
        {
            objectDb.SetParameterValue("DomainName", param.DomainName);
            objectDb.SetParameterValue("IsSSL", param.IsSSL);
            objectDb.SetParameterValue("UserID", param.UserId);
            objectDb.SetParameterValue("IP", param.Ip);

            AssignResultOutputParam(objectDb);
        }

        private static DomainInfo BuildSubAffDomainInfoItem(IDataReader reader)
        {
            return new DomainInfo
            {
                DomainId = Convert.ToInt32(reader["DomainId"]),
                DomainName = Convert.ToString(reader["DomainName"]),
                IsSSL = reader.GetVal<bool>("IsSSL"),
                Creator = Convert.ToString(reader["Creator"]),
                CreatedDate = reader["CreateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreateDate"])
            };
        }
    }
}
