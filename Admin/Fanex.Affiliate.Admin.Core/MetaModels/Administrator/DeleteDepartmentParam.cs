using Fanex.Affiliate.Admin.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class DeleteDepartmentParam : UserInfoParam
    {
        [Range(Validation.MinId, Validation.MaxId)]
        public int DepartmentId { get; set; }
    }
}
