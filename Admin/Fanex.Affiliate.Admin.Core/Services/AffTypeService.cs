using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Repository;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services
{
    public class AffTypeService : BaseRepository, IAffTypeService
    {
        public IEnumerable<AffTypeInfo> GetAffTypeList()
        {
            const string SpKey = "Aff_AffTypeSel";

            var result = new List<AffTypeInfo>();

            using (var objectDb = GetObjectDb(SpKey))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new AffTypeInfo
                        {
                            AffTypeId = Convert.ToInt32(reader["TypeId"]),
                            AffTypeName = Convert.ToString(reader["TypeName"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
