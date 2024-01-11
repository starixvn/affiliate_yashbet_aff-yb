using Fanex.Affiliate.User.Core.Constants;
using Fanex.Data;
using System;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class BaseRepository
    {
        private const string ERR_CODE = "ErrCode";
        private const string ERR_MSG = "ErrMsg";

        protected BaseRepository() { }

        protected static (int ErrCode, string ErrMsg) GetResult(IObjectDb objectDb)
        {
            var code = Convert.ToInt32(objectDb.GetOutputParams()[ERR_CODE]);
            var msg = Convert.ToString(objectDb.GetOutputParams()[ERR_MSG]);
            return (code, msg);
        }

        protected static void AddOutputParam(IObjectDb objectDb)
        {
            objectDb.SetParameterValue(ERR_CODE, DefaultValue.Int);
            objectDb.SetParameterValue(ERR_MSG, DefaultValue.String);
        }
    }
}
