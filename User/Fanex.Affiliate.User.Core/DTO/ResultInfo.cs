using System;

namespace Fanex.Affiliate.User.Core.DTO
{
    [Serializable]
    public class ResultInfo
    {
        public bool IsOk { get; set; }

        public string Message { get; set; }
    }

    [Serializable]
    public class ResultInfo<T> : ResultInfo
    {
        public T Data { get; set; }
    }
}