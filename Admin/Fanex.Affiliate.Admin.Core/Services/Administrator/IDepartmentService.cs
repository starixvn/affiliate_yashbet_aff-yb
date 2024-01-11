using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Administrator;
using Fanex.Affiliate.Admin.Core.MetaModels.Administrator;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Administrator
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentInfo> GetDepartmentList();

        DepartmentInfo GetDepartmentDetail(int selectedDepartmentId);

        ResultInfo InsertDepartment(InsertUpdateDepartmentParam param);

        ResultInfo UpdateDepartment(InsertUpdateDepartmentParam param);

        ResultInfo DeleteDepartment(DeleteDepartmentParam param);

        IEnumerable<PolicyInfo> GetRolePolicy(int departmentId);

        ResultInfo UpdateRolePolicy(UpdatePolicyParam param);
    }
}
