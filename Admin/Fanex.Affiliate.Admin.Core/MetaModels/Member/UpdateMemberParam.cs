using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Member
{
    public class UpdateMemberParam : UserInfoParam
    {
        [Range(Validation.MinId, Validation.MaxId, ErrorMessage = "The member is invalid!")]
        public int MemberId { get; set; }

        public bool IsRemoved { get; set; }

        public bool IsInclude { get; set; }
    }
}
