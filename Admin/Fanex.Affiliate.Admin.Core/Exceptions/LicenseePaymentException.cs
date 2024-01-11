using Fanex.Affiliate.Admin.Core.DTO;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Fanex.Affiliate.Admin.Core.Exceptions
{
    [Serializable]
    public class LicenseetException : Exception
    {
        public LicenseetException(object param, ResultInfo resultInfo)
            : base($"Error in lincensee api:\n" +
                $"Param: {JsonConvert.SerializeObject(param)}\n" +
                $"Error: {resultInfo.Code} - {resultInfo.Message}")
        { }

        public LicenseetException(object param, Exception innerExceptin)
            : base($"Error in lincensee api:\nParam: {JsonConvert.SerializeObject(param)}", innerExceptin)
        { }

        protected LicenseetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
