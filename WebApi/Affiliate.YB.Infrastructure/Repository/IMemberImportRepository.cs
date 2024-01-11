using Affiliate.YB.DTO;
using System;
using System.Threading.Tasks;

namespace Affiliate.YB.Repository
{
    public interface IMemberImportRepository
    {
        Task ImportMemberAsync(InsertMemberInfoCommand memberInfo);

        Task<DateTime> GetLastedMemberImportedTimeAsync(MemberImportedTimeInfo settingInfo);

        Task<string> GetSiteNameAsync(SiteNameInfo settingInfo);
    }
}