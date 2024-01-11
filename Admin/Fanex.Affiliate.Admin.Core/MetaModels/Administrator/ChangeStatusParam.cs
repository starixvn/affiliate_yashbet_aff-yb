using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class ChangeStatusParam : UserInfoParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int SelectedUserId { get; set; }

        public bool IsOpen { get; set; }
    }
}
