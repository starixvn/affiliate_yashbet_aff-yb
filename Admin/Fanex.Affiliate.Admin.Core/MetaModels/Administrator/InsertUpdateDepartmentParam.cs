
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Administrator
{
    public class InsertUpdateDepartmentParam : UserInfoParam
    {
        public int DepartmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        public string Description { get; set; }
    }
}
