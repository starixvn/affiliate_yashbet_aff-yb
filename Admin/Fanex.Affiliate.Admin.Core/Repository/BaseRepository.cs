using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Data;
using Fanex.Data.SqlServer;
using System;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public class BaseRepository
    {
        protected internal virtual IObjectDb GetObjectDb(string spName, int timeout = 120)
        {
            return new SqlObjectDb(spName, timeout);
        }

        protected static void AssignResultOutputParam(IObjectDb objectDb)
        {
            objectDb.SetParameterValue("ErrCode", DefaultValue.Int);
            objectDb.SetParameterValue("ErrMsg", DefaultValue.String);
        }

        protected static ResultInfo GetResultInfo(IObjectDb objectDb)
        {
            var code = objectDb.GetParameterValue("ErrCode");

            return new ResultInfo
            {
                Code = code == DBNull.Value ? 0 : Convert.ToInt32(code),
                Message = Convert.ToString(objectDb.GetParameterValue("ErrMsg"))
            };
        }

        protected static int GetTotalRows(IObjectDb objectDb)
        {
            var totalRows = objectDb.GetParameterValue("NRows");

            return totalRows == DBNull.Value ? 0 : Convert.ToInt32(totalRows);
        }
    }
}