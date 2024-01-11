using System;

namespace Fanex.Affiliate.Admin.Core.DTO.Common
{
    [Serializable]
    public class UserPermission
    {
        public string FunctionName { get; set; }

        public bool IsHost { get; set; }
    }
}